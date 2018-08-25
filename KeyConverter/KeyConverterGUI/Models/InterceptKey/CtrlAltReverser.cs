using InterceptKeyboardLib.Input;
using InterceptKeyboardLib.KeyMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyConverterGUI.Models.InterceptKey
{
    public class CtrlAltReverser : InterceptKeys
    {
        #region Singleton
        public static CtrlAltReverser Instance { get; } = new CtrlAltReverser();
        private CtrlAltReverser()
        {
        }
        #endregion

        protected override IntPtr KeyDownAction(Key pushedKey, Func<IntPtr> defaultReturnFunc)
        {
            return base.KeyDownAction(pushedKey, defaultReturnFunc);
        }

        protected override void KeyUpAction(Key upKey)
        {
            base.KeyUpAction(upKey);
        }
    }
}
