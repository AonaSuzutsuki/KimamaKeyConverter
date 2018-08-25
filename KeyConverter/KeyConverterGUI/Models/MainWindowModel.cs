using CommonStyleLib.Models;
using InterceptKeyboardLib.Input;
using InterceptKeyboardLib.KeyMap;
using KeyConverterGUI.Models.InterceptKey;
using KeyConverterGUI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KeyConverterGUI.Models
{
    public class MainWindowModel : ModelBase, IDisposable
    {
        #region Constants
        private const string DISABLED_TEXT = "Now Disabled";
        private const string ENABLED_TEXT = "Now Enabled";
        #endregion

        #region Fields
        private string buttonText = DISABLED_TEXT;

        private KeyboardWindow keymapping;
        private CtrlAltReverser interceptKeys;
        private bool isEnabled = false;

        private Dictionary<OriginalKey, OriginalKey> keyMap = new Dictionary<OriginalKey, OriginalKey>()
                {
                    { OriginalKey.LeftCtrl, OriginalKey.LeftAlt },
                    { OriginalKey.LeftAlt, OriginalKey.LeftCtrl },
                };
        #endregion

        #region Properties
        public string ButtonText
        {
            get => buttonText;
            set => SetProperty(ref buttonText, value);
        }
        #endregion

        public void EnabledOrDisabled()
        {
            if (!isEnabled)
            {
                interceptKeys = CtrlAltReverser.Instance;
                interceptKeys.KeyMap = keyMap;
                interceptKeys.Initialize();
                
                var processes = Process.GetProcessesByName("Client");
                if (processes.Length > 0)
                    interceptKeys.SpecificProcessId = processes[0].Id;

                isEnabled = true;
                ButtonText = ENABLED_TEXT;
            }
            else
            {
                interceptKeys.Dispose();

                isEnabled = false;
                ButtonText = DISABLED_TEXT;
            }
        }

        public void OpenKeyMapping()
        {
            keymapping = new KeyboardWindow(keyMap);
            keymapping.ShowDialog();
        }


        #region IDisposable
        public void Dispose()
        {
            interceptKeys?.Dispose();
            keymapping?.Dispose();
        }
        #endregion
    }
}
