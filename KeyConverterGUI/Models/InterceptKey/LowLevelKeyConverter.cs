using LowLevelKeyboardLib.Input;
using LowLevelKeyboardLib.KeyMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyConverterGUI.Models.InterceptKey
{
    public class LowLevelKeyConverter : LowLevelKeyDetector
    {
        #region Singleton
        public static LowLevelKeyConverter Instance { get; } = new LowLevelKeyConverter();
        private LowLevelKeyConverter()
        {
        }
        #endregion

        #region Win32API Methods
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("psapi.dll", CharSet = CharSet.Ansi)]
        private static extern uint GetModuleBaseName(IntPtr hWnd, IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder lpBaseName, uint nSize);

        [DllImport("psapi.dll", CharSet = CharSet.Ansi)]
        private static extern uint GetModuleFileNameEx(IntPtr hWnd, IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder lpBaseName, uint nSize);

        #endregion

        #region Properties
        public int SpecificProcessId { get; set; } = 0;
        public HashSet<string> ProcessNames { get; set; }
        #endregion

        public Dictionary<OriginalKey, OriginalKey> KeyMap { get; set; } = new Dictionary<OriginalKey, OriginalKey>();

        public override void Initialize()
        {
            ProcessNames = new HashSet<string>();

            base.Initialize();
        }

        private bool IsProcessName()
        {
            if (ProcessNames != null && ProcessNames.Count > 0)
            {
                var handle = GetForegroundWindow();
                var threadId = GetWindowThreadProcessId(handle, out var processId);

                var hnd = OpenProcess(0x0400 | 0x0010 , false, processId);

                var buffer2 = new StringBuilder(255);
                GetModuleFileNameEx(hnd, IntPtr.Zero, buffer2, (uint) buffer2.Capacity);

                CloseHandle(hnd);
                
                var fullPath = buffer2.ToString().ToLower();

                return ProcessNames.Contains(fullPath);
            }

            return true;
        }

        protected override IntPtr KeyDownFunction(OriginalKey pushedKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            if (!isVirtualInput && IsProcessName())
            {
                if (pushedKey == OriginalKey.None)
                    return new IntPtr(1);

                if (KeyMap.ContainsKey(pushedKey))
                {
                    var input = KeyMap[pushedKey];
                    return InputKey(pushedKey, input);
                }
            }
            
            return base.KeyDownFunction(pushedKey, isVirtualInput, defaultReturnFunc);
        }

        protected override IntPtr KeyUpFunction(OriginalKey upKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            if (inkeys.ContainsKey(upKey))
            {
                var inkey = inkeys[upKey];
                inkeys.Remove(upKey);
                input.KeyUp(inkey);
            }

            return base.KeyUpFunction(upKey, isVirtualInput, defaultReturnFunc);
        }
    }
}
