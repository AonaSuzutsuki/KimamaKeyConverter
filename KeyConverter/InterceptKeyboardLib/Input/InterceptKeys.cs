using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static InterceptKeyboardLib.Input.InterceptInput;
using InterceptKeyboardLib.KeyMap;

namespace InterceptKeyboardLib.Input
{
    public class InterceptKeys : IDisposable
    {
        #region Win32API Constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private LowLevelKeyboardProc proc;
        private IntPtr hookID = IntPtr.Zero;
        #endregion

        #region Win32API Structures
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_SCANCODE = 0x0008,
            KEYEVENTF_UNICODE = 0x0004,
        }
        #endregion

        #region Win32API Methods
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        #endregion

        #region Delegates
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Fields
        private InterceptInput input = new InterceptInput();
        private bool isIntercepted = false;

        private Dictionary<Key, INPUT> inkeys = new Dictionary<Key, INPUT>();
        #endregion

        #region Properties
        public int SpecificProcessId { get; set; } = 0;
        #endregion

        #region Singleton
        public static InterceptKeys Instance { get; } = new InterceptKeys();
        private InterceptKeys()
        {
        }
        #endregion

        public void Initialize()
        {
            if (!isIntercepted)
            {
                proc = HookProcedure;
                hookID = SetHook(proc);
                inkeys = new Dictionary<Key, INPUT>();
                isIntercepted = true;
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (SpecificProcessId > 0)
            {
                IntPtr handle = GetForegroundWindow();
                uint threadID = GetWindowThreadProcessId(handle, out var _processID);
                int processId = Convert.ToInt32(_processID);
                if (processId == SpecificProcessId)
                    return KeyboardProcedure(nCode, wParam, lParam);
            }
            else if (SpecificProcessId == 0)
            {
                return KeyboardProcedure(nCode, wParam, lParam);
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        private IntPtr KeyboardProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                var key = KeyMapConverter.KeyCodeToKey(vkCode);
                if (kb.dwExtraInfo.ToUInt32() != InterceptInput.MAGIC_NUMBER)
                {
                    IntPtr inputFunc(Key argKey)
                    {
                        var inputKey = KeyMapConverter.KeyToCode(argKey);
                        var inkey = input.KeyDown(inputKey);
                        if (!inkeys.ContainsKey(key))
                            inkeys.Add(key, inkey);
                        return new IntPtr(1);
                    }

                    if (key.Equals(Key.LeftAlt))
                        return inputFunc(Key.LeftCtrl);
                    else if (key.Equals(Key.LeftCtrl))
                        return inputFunc(Key.LeftAlt);
                }
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                var key = KeyMapConverter.KeyCodeToKey(vkCode);
                if (inkeys.ContainsKey(key))
                {
                    var inkey = inkeys[key];
                    inkeys.Remove(key);
                    input.KeyUp(inkey);
                }

                foreach (var k in inkeys.Keys)
                {
                    Console.WriteLine(k);
                }
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }


        #region IDisposable
        public void Dispose()
        {
            AllKeyUp();
            UnhookWindowsHookEx(hookID);
            isIntercepted = false;
        }
        public void AllKeyUp()
        {
            var keys = inkeys.Values;
            foreach (var key in keys)
                input.KeyUp(key);
        }
        #endregion
    }
}
