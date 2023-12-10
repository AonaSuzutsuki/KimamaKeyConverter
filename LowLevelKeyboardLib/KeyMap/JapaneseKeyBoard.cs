using System;
using System.Text;

namespace LowLevelKeyboardLib.KeyMap
{
    public class JapaneseKeyBoard : KeyBoard
    {
        public JapaneseKeyBoard()
        {
            KeyCodeDictionary.Add(KeyEnum.None, -0x1);
            KeyCodeDictionary.Add(KeyEnum.Unknown, 0x0);

            KeyCodeDictionary.Add(KeyEnum.Back, 0x8);
            KeyCodeDictionary.Add(KeyEnum.Tab, 0x9);
            KeyCodeDictionary.Add(KeyEnum.Enter, 0xD);
            KeyCodeDictionary.Add(KeyEnum.ESC, 0x1B);
            KeyCodeDictionary.Add(KeyEnum.Henkan, 0x1C);
            KeyCodeDictionary.Add(KeyEnum.Muhenkan, 0x1D);
            KeyCodeDictionary.Add(KeyEnum.Space, 0x20);
            KeyCodeDictionary.Add(KeyEnum.Home, 0x24);
            KeyCodeDictionary.Add(KeyEnum.Insert, 0x2D);
            KeyCodeDictionary.Add(KeyEnum.Delete, 0x2E);
            KeyCodeDictionary.Add(KeyEnum.PrintScreen, 0x2C);
            KeyCodeDictionary.Add(KeyEnum.ScrollLock, 0x91);
            KeyCodeDictionary.Add(KeyEnum.Pause, 0x13);
            KeyCodeDictionary.Add(KeyEnum.End, 0x23);
            KeyCodeDictionary.Add(KeyEnum.PageUp, 0x21);
            KeyCodeDictionary.Add(KeyEnum.PageDown, 0x22);

            KeyCodeDictionary.Add(KeyEnum.Left, 0x25);
            KeyCodeDictionary.Add(KeyEnum.Up, 0x26);
            KeyCodeDictionary.Add(KeyEnum.Right, 0x27);
            KeyCodeDictionary.Add(KeyEnum.Down, 0x28);

            KeyCodeDictionary.Add(KeyEnum.D0, 0x30);
            KeyCodeDictionary.Add(KeyEnum.D1, 0x31);
            KeyCodeDictionary.Add(KeyEnum.D2, 0x32);
            KeyCodeDictionary.Add(KeyEnum.D3, 0x33);
            KeyCodeDictionary.Add(KeyEnum.D4, 0x34);
            KeyCodeDictionary.Add(KeyEnum.D5, 0x35);
            KeyCodeDictionary.Add(KeyEnum.D6, 0x36);
            KeyCodeDictionary.Add(KeyEnum.D7, 0x37);
            KeyCodeDictionary.Add(KeyEnum.D8, 0x38);
            KeyCodeDictionary.Add(KeyEnum.D9, 0x39);

            KeyCodeDictionary.Add(KeyEnum.A, 0x41);
            KeyCodeDictionary.Add(KeyEnum.B, 0x42);
            KeyCodeDictionary.Add(KeyEnum.C, 0x43);
            KeyCodeDictionary.Add(KeyEnum.D, 0x44);
            KeyCodeDictionary.Add(KeyEnum.E, 0x45);
            KeyCodeDictionary.Add(KeyEnum.F, 0x46);
            KeyCodeDictionary.Add(KeyEnum.G, 0x47);
            KeyCodeDictionary.Add(KeyEnum.H, 0x48);
            KeyCodeDictionary.Add(KeyEnum.I, 0x49);
            KeyCodeDictionary.Add(KeyEnum.J, 0x4A);
            KeyCodeDictionary.Add(KeyEnum.K, 0x4B);
            KeyCodeDictionary.Add(KeyEnum.L, 0x4C);
            KeyCodeDictionary.Add(KeyEnum.M, 0x4D);
            KeyCodeDictionary.Add(KeyEnum.N, 0x4E);
            KeyCodeDictionary.Add(KeyEnum.O, 0x4F);
            KeyCodeDictionary.Add(KeyEnum.P, 0x50);
            KeyCodeDictionary.Add(KeyEnum.Q, 0x51);
            KeyCodeDictionary.Add(KeyEnum.R, 0x52);
            KeyCodeDictionary.Add(KeyEnum.S, 0x53);
            KeyCodeDictionary.Add(KeyEnum.T, 0x54);
            KeyCodeDictionary.Add(KeyEnum.U, 0x55);
            KeyCodeDictionary.Add(KeyEnum.V, 0x56);
            KeyCodeDictionary.Add(KeyEnum.W, 0x57);
            KeyCodeDictionary.Add(KeyEnum.X, 0x58);
            KeyCodeDictionary.Add(KeyEnum.Y, 0x59);
            KeyCodeDictionary.Add(KeyEnum.Z, 0x5A);

            KeyCodeDictionary.Add(KeyEnum.NumLock, 0x90);
            KeyCodeDictionary.Add(KeyEnum.N0, 0x60);
            KeyCodeDictionary.Add(KeyEnum.N1, 0x61);
            KeyCodeDictionary.Add(KeyEnum.N2, 0x62);
            KeyCodeDictionary.Add(KeyEnum.N3, 0x63);
            KeyCodeDictionary.Add(KeyEnum.N4, 0x64);
            KeyCodeDictionary.Add(KeyEnum.N5, 0x65);
            KeyCodeDictionary.Add(KeyEnum.N6, 0x66);
            KeyCodeDictionary.Add(KeyEnum.N7, 0x67);
            KeyCodeDictionary.Add(KeyEnum.N8, 0x68);
            KeyCodeDictionary.Add(KeyEnum.N9, 0x69);
            KeyCodeDictionary.Add(KeyEnum.NDiv, 0x6F);
            KeyCodeDictionary.Add(KeyEnum.NMul, 0x6A);
            KeyCodeDictionary.Add(KeyEnum.NMinus, 0x6D);
            KeyCodeDictionary.Add(KeyEnum.NPlus, 0x6B);
            KeyCodeDictionary.Add(KeyEnum.NDot, 0x6E);

            KeyCodeDictionary.Add(KeyEnum.LeftWindows, 0x5B);
            KeyCodeDictionary.Add(KeyEnum.RightWindows, 0x5C);
            KeyCodeDictionary.Add(KeyEnum.Menu, 0x5D);

            KeyCodeDictionary.Add(KeyEnum.F1, 0x70);
            KeyCodeDictionary.Add(KeyEnum.F2, 0x71);
            KeyCodeDictionary.Add(KeyEnum.F3, 0x72);
            KeyCodeDictionary.Add(KeyEnum.F4, 0x73);
            KeyCodeDictionary.Add(KeyEnum.F5, 0x74);
            KeyCodeDictionary.Add(KeyEnum.F6, 0x75);
            KeyCodeDictionary.Add(KeyEnum.F7, 0x76);
            KeyCodeDictionary.Add(KeyEnum.F8, 0x77);
            KeyCodeDictionary.Add(KeyEnum.F9, 0x78);
            KeyCodeDictionary.Add(KeyEnum.F10, 0x79);
            KeyCodeDictionary.Add(KeyEnum.F11, 0x7A);
            KeyCodeDictionary.Add(KeyEnum.F12, 0x7B);

            KeyCodeDictionary.Add(KeyEnum.LeftShift, 0xA0);
            KeyCodeDictionary.Add(KeyEnum.RightShift, 0xA1);
            KeyCodeDictionary.Add(KeyEnum.LeftCtrl, 0xA2);
            KeyCodeDictionary.Add(KeyEnum.RightCtrl, 0xA3);
            KeyCodeDictionary.Add(KeyEnum.RightAlt, 0xA5);
            KeyCodeDictionary.Add(KeyEnum.LeftAlt, 0xA4);
            KeyCodeDictionary.Add(KeyEnum.Multiply, 0xBA);
            KeyCodeDictionary.Add(KeyEnum.Plus, 0xBB);
            KeyCodeDictionary.Add(KeyEnum.Comma, 0xBC);
            KeyCodeDictionary.Add(KeyEnum.Equal, 0xBD);
            KeyCodeDictionary.Add(KeyEnum.Period, 0xBE);
            KeyCodeDictionary.Add(KeyEnum.Question, 0xBF);
            KeyCodeDictionary.Add(KeyEnum.AtMark, 0xC0);
            KeyCodeDictionary.Add(KeyEnum.LeftBrace, 0xDB);
            KeyCodeDictionary.Add(KeyEnum.VerticalBar, 0xDC);
            KeyCodeDictionary.Add(KeyEnum.RightBrace, 0xDD);
            KeyCodeDictionary.Add(KeyEnum.Tilde, 0xDE);

            KeyCodeDictionary.Add(KeyEnum.UnderBar, 0xE2);

            KeyCodeDictionary.Add(KeyEnum.Caps, 0xF0);
            KeyCodeDictionary.Add(KeyEnum.Kana, 0xF2);
            KeyCodeDictionary.Add(KeyEnum.Hankaku, 0xF3);
            KeyCodeDictionary.Add(KeyEnum.Zenkaku, 0xF4);
            KeyCodeDictionary.Add(KeyEnum.HankakuZenkaku, 0x270F);

            Init();
        }
    }
}
