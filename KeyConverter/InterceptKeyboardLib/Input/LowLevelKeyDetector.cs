using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static LowLevelKeyboardLib.Input.LowLevelKeyInput;
using LowLevelKeyboardLib.KeyMap;

namespace LowLevelKeyboardLib.Input
{
    public class LowLevelKeyDetector : AbstractLowLevelKeyDetector, IDisposable
    {
        #region Fields
        private bool isIntercepted = false;

        protected LowLevelKeyInput input = new LowLevelKeyInput();
        protected Dictionary<OriginalKey, INPUT> inkeys = new Dictionary<OriginalKey, INPUT>();
        #endregion

        #region Properties
        #endregion

        #region InputEvent
        public class OriginalKeyEventArg : EventArgs
        {
            public int KeyCode { get; }
            public OriginalKey Key { get; }
            public bool IsVirtualInput { get; }

            public OriginalKeyEventArg(int keyCode, OriginalKey key, bool isVirtualInput)
            {
                KeyCode = keyCode;
                Key = key;
                IsVirtualInput = isVirtualInput;
            }
        }
        public delegate void KeyEventHandler(object sender, OriginalKeyEventArg e);
        public event KeyEventHandler KeyDownEvent;
        public event KeyEventHandler KeyUpEvent;

        protected void OnKeyDownEvent(int keyCode, OriginalKey key, bool isVirtualInput)
        {
            KeyDownEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, key, isVirtualInput));
        }

        protected void OnKeyUpEvent(int keyCode, OriginalKey key, bool isVirtualInput)
        {
            KeyUpEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, key, isVirtualInput));
        }
        #endregion

        /// <summary>
        /// Initialize WindowsHook and start to intercept.
        /// </summary>
        public void Initialize()
        {
            this.Hook();
        }

        public override void Hook()
        {
            if (!isIntercepted)
            {
                base.Hook();
                inkeys = new Dictionary<OriginalKey, INPUT>();
                isIntercepted = true;
            }
            else
                throw new AlreadyInterceptedException("Can hook only once.");
        }

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                var key = KeyMapConverter.KeyCodeToKey(vkCode);
                var isVirtualInput = kb.dwExtraInfo.ToUInt32() == MAGIC_NUMBER;
                OnKeyDownEvent(vkCode, key, isVirtualInput);
                return KeyDownAction(key, isVirtualInput, () => base.HookProcedure(nCode, wParam, lParam));
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                var key = KeyMapConverter.KeyCodeToKey(vkCode);
                var isVirtualInput = kb.dwExtraInfo.ToUInt32() == MAGIC_NUMBER;
                OnKeyUpEvent(vkCode, key, isVirtualInput);
                return KeyUpAction(key, isVirtualInput, () => base.HookProcedure(nCode, wParam, lParam));
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }

        /// <summary>
        /// Input the Key.
        /// </summary>
        /// <param name="pushedKey">Actually pushed key</param>
        /// <param name="destKey">Converted key</param>
        /// <returns></returns>
        protected IntPtr InputKey(OriginalKey pushedKey, OriginalKey destKey)
        {
            var inputKey = KeyMapConverter.KeyToCode(destKey);
            var inkey = input.KeyDown(inputKey);
            if (!inkeys.ContainsKey(pushedKey))
                inkeys.Add(pushedKey, inkey);
            return new IntPtr(1);
        }

        /// <summary>
        /// KeyDown method. It is a method for override.
        /// </summary>
        /// <param name="pushedKey">Actually pushed key</param>
        /// <param name="defaultReturnFunc">Default return func.</param>
        /// <returns></returns>
        protected virtual IntPtr KeyDownAction(OriginalKey pushedKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            return defaultReturnFunc();
        }

        /// <summary>
        /// KeyUp method. It is a method for override.
        /// </summary>
        /// <param name="upKey">Converted key</param>
        protected virtual IntPtr KeyUpAction(OriginalKey upKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            return defaultReturnFunc();
        }

        #region IDisposable
        /// <summary>
        /// Unhook WindowsHook and Execute to up all pushed key.
        /// </summary>
        public void Dispose()
        {
            AllKeyUp();
            base.UnHook();
            isIntercepted = false;
        }

        /// <summary>
        /// Execute to up all pushed key.
        /// </summary>
        public void AllKeyUp()
        {
            var keys = inkeys.Values;
            foreach (var key in keys)
                input.KeyUp(key);
        }
        #endregion
    }
}
