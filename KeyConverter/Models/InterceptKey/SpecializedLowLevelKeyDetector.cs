using LowLevelKeyboardLib.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LowLevelKeyboardLib.KeyMap;

namespace KeyConverterGUI.Models.InterceptKey
{
    public class SpecializedLowLevelKeyDetector : LowLevelKeyDetector
    {
        #region Win32API Methods
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        #endregion

        #region Properties
        public int SpecificProcessId { get; set; } = 0;
        #endregion

        public SpecializedLowLevelKeyDetector(KeyBoard keyBoard) : base(keyBoard)
        {

        }

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (SpecificProcessId > 0)
            {
                var handle = GetForegroundWindow();
                var threadId = GetWindowThreadProcessId(handle, out var _processID);
                var processId = Convert.ToInt32(_processID);
                if (processId == SpecificProcessId)
                {
                    base.HookProcedure(nCode, wParam, lParam);
                    return new IntPtr(1);
                }
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }
    }
}
