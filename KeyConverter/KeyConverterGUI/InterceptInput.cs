using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyConverterGUI
{
    public class InterceptInput
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT no;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };

        [DllImport("user32.dll")]
        private extern static void SendInput(int nInputs, ref INPUT pInputs, int cbsize);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        private extern static int MapVirtualKey(int wCode, int wMapType);

        private const int INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        public void Send(int key, bool isEXTEND)
        {
            /*
             * Keyを送る
             * 入力
             *     isEXTEND : 拡張キーかどうか
             */

            INPUT inp = new INPUT();

            // 押す
            inp.type = INPUT_KEYBOARD;
            inp.ki.wVk = (short)key;
            inp.ki.wScan = (short)MapVirtualKey(inp.ki.wVk, 0);
            inp.ki.dwFlags = ((isEXTEND) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN;
            inp.ki.time = 0;
            inp.ki.dwExtraInfo = 102;
            SendInput(1, ref inp, Marshal.SizeOf(inp));

            System.Threading.Thread.Sleep(100);

            // 離す
            inp.ki.dwFlags = ((isEXTEND) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref inp, Marshal.SizeOf(inp));
        }

        public INPUT KeyDown(int key)
        {
            INPUT inp = new INPUT();

            // 押す
            inp.type = INPUT_KEYBOARD;
            inp.ki.wVk = (short)key;
            inp.ki.wScan = (short)MapVirtualKey(inp.ki.wVk, 0);
            inp.ki.dwFlags = ((false) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN;
            inp.ki.time = 0;
            inp.ki.dwExtraInfo = 102;
            SendInput(1, ref inp, Marshal.SizeOf(inp));
            return inp;
        }

        public void KeyUp(INPUT input)
        {
            input.ki.dwFlags = ((false) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }
    }
}
