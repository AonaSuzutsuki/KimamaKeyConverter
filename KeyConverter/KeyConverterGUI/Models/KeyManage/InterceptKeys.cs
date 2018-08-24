using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static KeyConverterGUI.Models.KeyManage.InterceptInput;

namespace KeyConverterGUI.Models.KeyManage
{
    public class InterceptKeys : IDisposable
    {
        #region Win32API Constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
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
        private static InterceptInput input = new InterceptInput();
        private bool isIntercepted = false;
        #endregion

        #region Properties
        public static int SpecificProcessId { get; set; } = 0;
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
                _hookID = SetHook(_proc);
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

        private static INPUT inkey;
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            IntPtr func()
            {
                if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
                {
                    KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                    var vkCode = (int)kb.vkCode;
                    var key = KeyConverter.KeyCodeToKey(vkCode);
                    if (kb.dwExtraInfo.ToUInt32() != 102u)
                    {
                        IntPtr inputFunc(Key argKey)
                        {
                            var inputKey = KeyConverter.KeyToCode(argKey);
                            inkey = input.KeyDown(inputKey);
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
                    input.KeyUp(inkey);
                }

                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            if (SpecificProcessId > 0)
            {
                IntPtr handle = GetForegroundWindow();
                uint threadID = GetWindowThreadProcessId(handle, out var _processID);
                int processId = Convert.ToInt32(_processID);
                if (processId == SpecificProcessId)
                {
                    return func();
                }
            }
            else if (SpecificProcessId == 0)
                return func();

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        

        public void Dispose()
        {
            UnhookWindowsHookEx(_hookID);
            isIntercepted = false;
        }
    }
}
