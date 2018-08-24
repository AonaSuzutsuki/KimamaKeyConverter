using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyConverterGUI.Models.KeyManage
{
    public class KeyConverter
    {
        private static Dictionary<int, Key> map;

        static KeyConverter()
        {
            if (map != null)
                return;

            map = new Dictionary<int, Key>()
            {
                { 49, Key.D1 },
                { 50, Key.D2 },
                { 51, Key.D3 },
                { 52, Key.D4 },
                { 53, Key.D5 },
                { 54, Key.D6 },
                { 55, Key.D7 },
                { 56, Key.D8 },
                { 57, Key.D9 },
                { 48, Key.D0 },

                { 65, Key.A },
                { 66, Key.B },
                { 67, Key.C },
                { 68, Key.D },
                { 69, Key.E },
                { 70, Key.F },
                { 71, Key.G },
                { 72, Key.H },
                { 73, Key.I },
                { 74, Key.J },
                { 75, Key.K },
                { 76, Key.L },
                { 77, Key.M },
                { 78, Key.N },
                { 79, Key.O },
                { 80, Key.P },
                { 81, Key.Q },
                { 82, Key.R },
                { 83, Key.S },
                { 84, Key.T },
                { 85, Key.U },
                { 86, Key.V },
                { 87, Key.W },
                { 88, Key.X },
                { 89, Key.Y },
                { 90, Key.Z },

                { 8, Key.Back },
                { 9, Key.Tab },
                { 160, Key.LeftShift },
                { 161, Key.RightShift },
                { 162, Key.LeftCtrl },
                { 164, Key.LeftAlt },
                { 186, Key.Multiply },
                { 190, Key.OemPeriod },
                { 191, Key.OemQuestion },
                { 192, Key.OemTilde },
                { 243, Key.NoName }, //半
                { 244, Key.NoName }, //全
                { 45, Key.Insert },
                { 36, Key.Home },
                { 13, Key.Enter },
            };
        }

        public static Key KeyCodeToKey(int keycode)
        {
            if (map.ContainsKey(keycode))
                return map[keycode];

            return Key.NoName;
        }

        public static int KeyToCode(Key key)
        {
            var code = map.FirstOrDefault(x => x.Value == key).Key;
            return code;
        }
    }
}
