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
        public static OriginalKey KeyCodeToKey(int keycode)
        {
            if (Enum.IsDefined(typeof(OriginalKey), keycode))
                return (OriginalKey)keycode;

            return OriginalKey.Unknown;
        }

        public static int KeyToCode(OriginalKey key)
        {
            return (int)key;
        }
    }
}
