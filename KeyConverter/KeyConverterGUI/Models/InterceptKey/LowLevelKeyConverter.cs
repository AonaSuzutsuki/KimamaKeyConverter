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
        #endregion

        #region Properties
        public int SpecificProcessId { get; set; } = 0;
        public string ProcessName { get; set; }
        #endregion

        public Dictionary<OriginalKey, OriginalKey> KeyMap { get; set; } = new Dictionary<OriginalKey, OriginalKey>();

        public override void Initialize()
        {
            ProcessName = string.Empty;

            base.Initialize();
        }

        private bool IsProcessName()
        {
            if (!string.IsNullOrEmpty(ProcessName))
            {
                var handle = GetForegroundWindow();
                var threadId = GetWindowThreadProcessId(handle, out var _processID);
                var processId = Convert.ToInt32(_processID);

                var hnd = OpenProcess(0x0400 | 0x0010 , false, (uint)processId);

                var buffer = new StringBuilder(255);
                GetModuleBaseName(hnd, IntPtr.Zero, buffer, (uint)buffer.Capacity);

                CloseHandle(hnd);

                var processName = buffer.ToString().ToLower();
                Console.WriteLine(processName);

                return processName == ProcessName;
            }

            return true;
        }

        protected override IntPtr KeyDownFunction(OriginalKey pushedKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            if (!isVirtualInput && IsProcessName())
            {
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
