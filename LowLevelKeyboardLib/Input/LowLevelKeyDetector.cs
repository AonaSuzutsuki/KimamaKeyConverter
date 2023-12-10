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
    public class LowLevelKeyDetector : AbstractLowLevelKeyDetector
    {
        #region Fields
        private bool _isIntercepted = false;

        protected LowLevelKeyInput Input = new();

        protected Dictionary<KeyEnum, INPUT> Inkeys = new();

        protected KeyBoard KeyBoard;
        #endregion

        #region Properties
        #endregion

        #region InputEvent
        public class KeyEnumEventArg : EventArgs
        {
            public int KeyCode { get; }
            public KeyEnum Key { get; }
            public bool IsVirtualInput { get; }

            public KeyEnumEventArg(int keyCode, KeyEnum key, bool isVirtualInput)
            {
                KeyCode = keyCode;
                Key = key;
                IsVirtualInput = isVirtualInput;
            }
        }
        public delegate void KeyEventHandler(object sender, KeyEnumEventArg e);
        public event KeyEventHandler KeyDownEvent;
        public event KeyEventHandler KeyUpEvent;

        protected void OnKeyDownEvent(int keyCode, KeyEnum key, bool isVirtualInput)
        {
            KeyDownEvent?.Invoke(this, new KeyEnumEventArg(keyCode, key, isVirtualInput));
        }

        protected void OnKeyUpEvent(int keyCode, KeyEnum key, bool isVirtualInput)
        {
            KeyUpEvent?.Invoke(this, new KeyEnumEventArg(keyCode, key, isVirtualInput));
        }
        #endregion

        public LowLevelKeyDetector(KeyBoard keyBoard)
        {
            KeyBoard = keyBoard;
        }

        /// <summary>
        /// Initialize WindowsHook and start to intercept.
        /// </summary>
        public virtual void Initialize()
        {
            this.Hook();
        }

        public override void Hook()
        {
            if (!_isIntercepted)
            {
                base.Hook();
                Inkeys = new Dictionary<KeyEnum, INPUT>();
                _isIntercepted = true;
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
                var key = KeyBoard.GetKey(vkCode);

                var isVirtualInput = kb.dwExtraInfo == MAGIC_NUMBER;

                OnKeyDownEvent(vkCode, key, isVirtualInput);
                return KeyDownFunction(key, isVirtualInput, () => base.HookProcedure(nCode, wParam, lParam));
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                var key = KeyBoard.GetKey(vkCode);

                var isVirtualInput = kb.dwExtraInfo == MAGIC_NUMBER;

                OnKeyUpEvent(vkCode, key, isVirtualInput);
                return KeyUpFunction(key, isVirtualInput, () => base.HookProcedure(nCode, wParam, lParam));
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }

        /// <summary>
        /// Input the Key.
        /// </summary>
        /// <param name="pushedKey">Actually pushed key</param>
        /// <param name="destKey">Converted key</param>
        /// <returns></returns>
        protected IntPtr InputKey(KeyEnum pushedKey, KeyEnum destKey)
        {
            var inputKey = KeyBoard.GetKeyCode(destKey);
            var inkey = Input.KeyDown(inputKey);
            if (!Inkeys.ContainsKey(pushedKey))
                Inkeys.Add(pushedKey, inkey);
            return new IntPtr(1);
        }

        /// <summary>
        /// KeyDown method. It is a method for override.
        /// </summary>
        /// <param name="pushedKey">Actually pushed key</param>
        /// <param name="defaultReturnFunc">Default return func.</param>
        /// <returns></returns>
        protected virtual IntPtr KeyDownFunction(KeyEnum pushedKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            return defaultReturnFunc();
        }

        /// <summary>
        /// KeyUp method. It is a method for override.
        /// </summary>
        /// <param name="upKey">Converted key</param>
        protected virtual IntPtr KeyUpFunction(KeyEnum upKey, bool isVirtualInput, Func<IntPtr> defaultReturnFunc)
        {
            return defaultReturnFunc();
        }

        #region IDisposable

        /// <summary>
        /// Unhook WindowsHook and Execute to up all pushed key.
        /// </summary>
        public override void UnHook()
        {
            AllKeyUp();
            base.UnHook();
            _isIntercepted = false;
        }

        /// <summary>
        /// Execute to up all pushed key.
        /// </summary>
        public void AllKeyUp()
        {
            var keys = Inkeys.Values;
            foreach (var key in keys)
                Input.KeyUp(key);
        }
        #endregion
    }
}
