using CommonCoreLib.Ini;
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
        #endregion

        #region Fields
        private string buttonText = LangResource.Resources.Resources.UI_Disabled;
        private bool enabledBtEnabled = true;
        private bool keymappingBtEnabled = true;

        private KeyboardWindow keymapping;
        private LowLevelKeyConverter interceptKeys;
        private bool isEnabled = false;
        private bool isDetectMabinogi = true;

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
        public bool IsDetectMabinogi
        {
            get => isDetectMabinogi;
            set => SetProperty(ref isDetectMabinogi, value);
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

            LoadSetting();
        }

        public void EnabledOrDisabled()
        {
            if (!isEnabled)
            {
                interceptKeys = LowLevelKeyConverter.Instance;
                interceptKeys.KeyMap = keyMap;
                interceptKeys.Initialize();
                
                if (IsDetectMabinogi)
                {
                    var processes = Process.GetProcessesByName("Client");
                    if (processes.Length > 0)
                        interceptKeys.SpecificProcessId = processes[0].Id;
                }
                

                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("../Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["EnabledColor"] as SolidColorBrush);

                isEnabled = true;
                ButtonText = LangResource.Resources.Resources.UI_Enabled;
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
                ButtonText = LangResource.Resources.Resources.UI_Disabled;
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


        #region Setting
        public void SaveSetting()
        {
            var iniLoader = new IniLoader(Constants.IniFileName);
            iniLoader.SetValue("Main", "IsDetectMabinogi", IsDetectMabinogi);
        }
        public void LoadSetting()
        {
            var iniLoader = new IniLoader(Constants.IniFileName);
            var IsDetectMabinogi = iniLoader.GetValue("Main", "IsDetectMabinogi", true);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            SaveSetting();
            interceptKeys?.Dispose();
            keymapping?.Dispose();
        }
        #endregion
    }
}
