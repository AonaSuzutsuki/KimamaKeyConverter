using InterceptKeyboardLib.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyConverterGUI.Models.InterceptKey
{
    public class LowLevelKeyGetter : LowLevelKeyDetector
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

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (SpecificProcessId > 0)
            {
                IntPtr handle = GetForegroundWindow();
                uint threadID = GetWindowThreadProcessId(handle, out var _processID);
                int processId = Convert.ToInt32(_processID);
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
