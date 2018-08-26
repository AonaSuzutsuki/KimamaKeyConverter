using CommonStyleLib.Models;
using InterceptKeyboardLib.Input;
using InterceptKeyboardLib.KeyMap;
using KeyConverterGUI.Models.InterceptKey;
using KeyConverterGUI.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

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
        private bool enabledBtEnabled = true;
        private bool keymappingBtEnabled = true;

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
        public bool EnabledBtEnabled
        {
            get => enabledBtEnabled;
            set => SetProperty(ref enabledBtEnabled, value);
        }
        public bool KeymappingBtEnabled
        {
            get => keymappingBtEnabled;
            set => SetProperty(ref keymappingBtEnabled, value);
        }
        #endregion

        #region Actions
        public Action<SolidColorBrush> ChangeBaseBackground { get; set; } = null;
        #endregion

        public MainWindowModel()
        {
            string json = null;
            if (File.Exists(Constants.KeyMapFileName))
            {
                using (var fs = new FileStream(Constants.KeyMapFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        json = sr.ReadToEnd();
                    }
                }
            }

            if (!string.IsNullOrEmpty(json))
                keyMap = JsonConvert.DeserializeObject<Dictionary<OriginalKey, OriginalKey>>(json);
        }

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

                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("../Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["EnabledColor"] as SolidColorBrush);

                isEnabled = true;
                ButtonText = ENABLED_TEXT;
            }
            else
            {
                interceptKeys.Dispose();

                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/CommonStyleLib;component/Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["MainColor"] as SolidColorBrush);

                isEnabled = false;
                ButtonText = DISABLED_TEXT;
            }
            KeymappingBtEnabled = !isEnabled;
        }

        public void OpenKeyMapping()
        {
            EnabledBtEnabled = false;
            keymapping = new KeyboardWindow(keyMap);
            keymapping.ShowDialog();
            keymapping.Dispose();
            EnabledBtEnabled = true;

            var json = JsonConvert.SerializeObject(keyMap);
            using (var fs = new FileStream(Constants.KeyMapFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(json);
                }
            }
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
