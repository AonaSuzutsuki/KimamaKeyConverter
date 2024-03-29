﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LowLevelKeyboardLib.Input
{
    public class LowLevelKeyInput
    {
        #region Win32API Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public UNION_INPUT input;
        };


        [StructLayout(LayoutKind.Explicit)]
        public struct UNION_INPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT no;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        #endregion

        #region Win32API Methods
        [DllImport("user32.dll")]
        private static extern void SendInput(int nInputs, ref INPUT pInputs, int cbsize);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        private static extern int MapVirtualKey(int wCode, int wMapType);
        #endregion

        #region Win32API Constants
        private const int INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        #endregion

        #region Constants
        public static readonly IntPtr MAGIC_NUMBER = (IntPtr)0x10209;
        #endregion

        /// <summary>
        /// Execute to keydown the key.
        /// </summary>
        /// <param name="key">Key to keydown</param>
        /// <param name="isExtend">Is extend key</param>
        /// <returns></returns>
        public INPUT KeyDown(int key, bool isExtend = false)
        {
            INPUT input = new INPUT
            {
                type = INPUT_KEYBOARD,
                input = new UNION_INPUT
                {
                    ki = new KEYBDINPUT()
                    {
                        wVk = (short)key,
                        wScan = (short)MapVirtualKey((short)key, 0),
                        dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                        time = 0,
                        dwExtraInfo = MAGIC_NUMBER
                    }
                },
            };

            var size = Marshal.SizeOf(input);
            SendInput(1, ref input, size);
            return input;
        }

        /// <summary>
        /// Execute to keyup the key.
        /// </summary>
        /// <param name="input">Key to keyup</param>
        /// <param name="isExtend">Is extend key</param>
        public void KeyUp(INPUT input, bool isExtend = false)
        {
            input.input.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }
    }
}
