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

        public Dictionary<OriginalKey, OriginalKey> KeyMap { get; set; } = new Dictionary<OriginalKey, OriginalKey>();

        protected override IntPtr KeyDownAction(OriginalKey pushedKey, Func<IntPtr> defaultReturnFunc)
        {
            if (KeyMap.ContainsKey(pushedKey))
            {
                var input = KeyMap[pushedKey];
                return InputFunc(pushedKey, input);
            }

            return defaultReturnFunc();
        }

        protected override void KeyUpAction(OriginalKey upKey)
        {
            if (inkeys.ContainsKey(upKey))
            {
                var inkey = inkeys[upKey];
                inkeys.Remove(upKey);
                input.KeyUp(inkey);
            }
        }
    }
}
