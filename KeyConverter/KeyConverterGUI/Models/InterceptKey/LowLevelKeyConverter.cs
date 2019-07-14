using LowLevelKeyboardLib.Input;
using LowLevelKeyboardLib.KeyMap;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Properties
        public int SpecificProcessId { get; set; } = 0;
        #endregion

        public Dictionary<OriginalKey, OriginalKey> KeyMap { get; set; } = new Dictionary<OriginalKey, OriginalKey>();

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (SpecificProcessId > 0)
            {
                IntPtr handle = GetForegroundWindow();
                uint threadID = GetWindowThreadProcessId(handle, out var _processID);
                int processId = Convert.ToInt32(_processID);
                if (processId == SpecificProcessId)
                    return base.HookProcedure(nCode, wParam, lParam);
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }

        protected override IntPtr KeyDownFunction(OriginalKey pushedKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            if (!isVirtualInput)
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
