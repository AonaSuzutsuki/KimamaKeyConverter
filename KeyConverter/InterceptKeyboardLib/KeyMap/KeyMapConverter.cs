using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LowLevelKeyboardLib.KeyMap
{
    public class KeyMapConverter
    {
        /// <summary>
        /// Convert int keycode to OriginalKey.
        /// </summary>
        /// <param name="keycode">int keycode</param>
        /// <returns></returns>
        public static OriginalKey KeyCodeToKey(int keycode)
        {
            if (Enum.IsDefined(typeof(OriginalKey), keycode))
                return (OriginalKey)keycode;

            return OriginalKey.Unknown;
        }

        /// <summary>
        /// Convert OriginalKey to int keycode.
        /// </summary>
        /// <param name="key">OriginalKey</param>
        /// <returns></returns>
        public static int KeyToCode(OriginalKey key)
        {
            return (int)key;
        }
    }
}
