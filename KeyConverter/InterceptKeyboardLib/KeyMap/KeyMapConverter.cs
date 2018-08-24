using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterceptKeyboardLib.KeyMap
{
    public class KeyMapConverter
    {
        public static Key KeyCodeToKey(int keycode)
        {
            if (Enum.IsDefined(typeof(Key), keycode))
                return (Key)keycode;

            return Key.Unknown;
        }

        public static int KeyToCode(Key key)
        {
            return (int)key;
        }
    }
}
