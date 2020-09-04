using CommonCoreLib.Ini;
using CommonStyleLib.Models;
using LowLevelKeyboardLib.Input;
using LowLevelKeyboardLib.KeyMap;
using KeyConverterGUI.Models.InterceptKey;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
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

        private LowLevelKeyConverter interceptKeys;
        private bool isEnabled = false;
        private bool isDetectMabinogi = true;
        private bool isDetectMabinogiEnabled = true;

        private readonly Dictionary<OriginalKey, OriginalKey> keyMap = new Dictionary<OriginalKey, OriginalKey>()
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

        public bool IsDetectMabinogiEnabled
        {
            get => isDetectMabinogiEnabled;
            set => SetProperty(ref isDetectMabinogiEnabled, value);
        }

        public HashSet<string> DetectProcesses { get; set; } = new HashSet<string>();
        #endregion

        #region Actions
        public Action<SolidColorBrush> ChangeBaseBackground { get; set; } = null;
        #endregion

        public MainWindowModel()
        {
            if (File.Exists(Constants.KeyMapFileName))
            {
                var json = File.ReadAllText(Constants.KeyMapFileName);
                if (!string.IsNullOrEmpty(json))
                    keyMap = JsonConvert.DeserializeObject<Dictionary<OriginalKey, OriginalKey>>(json);
            }

            LoadDetectProcesses();

            LoadSetting();
        }

        public void LoadDetectProcesses()
        {
            if (File.Exists(Constants.DetectProcessesFileName))
            {
                var json = File.ReadAllText(Constants.DetectProcessesFileName);
                if (!string.IsNullOrEmpty(json))
                    SetLowerHashSet(JsonConvert.DeserializeObject<HashSet<string>>(json));
            }
        }

        private static HashSet<string> ConvertLowerHashSet(IEnumerable<string> enumerable) =>
            new HashSet<string>(from x in enumerable select x.ToLower());

        public void SetLowerHashSet(HashSet<string> hashSet)
        {
            DetectProcesses = ConvertLowerHashSet(hashSet);
        }

        public void EnabledOrDisabled()
        {
            if (!isEnabled)
            {
                interceptKeys = LowLevelKeyConverter.Instance;
                interceptKeys.KeyMap = keyMap;
                interceptKeys.Initialize();

                if (IsDetectMabinogi)
                    interceptKeys.ProcessNames = DetectProcesses;
                
                
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("../Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["EnabledColor"] as SolidColorBrush);

                isEnabled = true;
                IsDetectMabinogiEnabled = false;
                ButtonText = LangResource.Resources.Resources.UI_Enabled;
            }
            else
            {
                interceptKeys.UnHook();

                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/CommonStyleLib;component/Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["MainColor"] as SolidColorBrush);

                isEnabled = false;
                IsDetectMabinogiEnabled = true;
                ButtonText = LangResource.Resources.Resources.UI_Disabled;
            }
            KeymappingBtEnabled = !isEnabled;
        }

        public KeyboardWindowModel CreateKeyboardWindowModel() => new KeyboardWindowModel(keyMap);

        public void SaveKeyMap()
        {
            var json = JsonConvert.SerializeObject(keyMap);
            using var fs = new FileStream(Constants.KeyMapFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(json);
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
            IsDetectMabinogi = iniLoader.GetValue("Main", "IsDetectMabinogi", true);
        }
        #endregion

        #region IDisposable
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            
            if (disposing)
            {
                SaveSetting();
                interceptKeys?.UnHook();
            }

            disposed = true;
        }
        #endregion
    }
}
