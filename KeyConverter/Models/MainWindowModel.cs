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
using KeyConverterGUI.Models.Data;

namespace KeyConverterGUI.Models
{
    public class MainWindowModel : ModelBase, IDisposable
    {
        #region Constants
        #endregion

        #region Fields
        private string _buttonText = LangResource.Resources.Resources.UI_Disabled;
        private bool _enabledBtEnabled = true;
        private bool _keymappingBtEnabled = true;

        private LowLevelKeyConverter _interceptKeys;
        private bool _isEnabled;
        private bool _isDetectMabinogi = true;
        private bool _isDetectMabinogiEnabled = true;

        private readonly Dictionary<KeyEnum, KeyEnum> _keyMap = new()
                {
                    { KeyEnum.LeftCtrl, KeyEnum.LeftAlt },
                    { KeyEnum.LeftAlt, KeyEnum.LeftCtrl }
                };
        #endregion

        #region Properties
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }
        public bool EnabledBtEnabled
        {
            get => _enabledBtEnabled;
            set => SetProperty(ref _enabledBtEnabled, value);
        }
        public bool KeymappingBtEnabled
        {
            get => _keymappingBtEnabled;
            set => SetProperty(ref _keymappingBtEnabled, value);
        }
        public bool IsDetectMabinogi
        {
            get => _isDetectMabinogi;
            set => SetProperty(ref _isDetectMabinogi, value);
        }

        public bool IsDetectMabinogiEnabled
        {
            get => _isDetectMabinogiEnabled;
            set => SetProperty(ref _isDetectMabinogiEnabled, value);
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
                {
                    var jsonObject = JsonConvert.DeserializeObject<SavedJson>(json);
                    if (jsonObject is { KeyMaps: { } })
                    {
                        _keyMap = jsonObject.KeyMaps;
                    }
                }
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
            if (!_isEnabled)
            {
                _interceptKeys = new LowLevelKeyConverter(new JapaneseKeyBoard());
                _interceptKeys.KeyMap = _keyMap;
                _interceptKeys.Initialize();

                if (IsDetectMabinogi)
                    _interceptKeys.ProcessNames = DetectProcesses;
                
                
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("../Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["EnabledColor"] as SolidColorBrush);

                _isEnabled = true;
                IsDetectMabinogiEnabled = false;
                ButtonText = LangResource.Resources.Resources.UI_Enabled;
            }
            else
            {
                _interceptKeys.Dispose();

                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/CommonStyleLib;component/Styles/Constants.xaml", UriKind.Relative)
                };
                ChangeBaseBackground?.Invoke(resourceDictionary["MainColor"] as SolidColorBrush);

                _isEnabled = false;
                IsDetectMabinogiEnabled = true;
                ButtonText = LangResource.Resources.Resources.UI_Disabled;
            }
            KeymappingBtEnabled = !_isEnabled;
        }

        public KeyboardWindowModel CreateKeyboardWindowModel() => new KeyboardWindowModel(_keyMap);

        public void SaveKeyMap()
        {
            var jsonObject = new SavedJson
            {
                Layout = KeyboardLayout.Jis,
                KeyMaps = _keyMap
            };

            var json = JsonConvert.SerializeObject(jsonObject);
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
        private bool _disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            
            if (disposing)
            {
                SaveSetting();
                _interceptKeys?.UnHook();
            }

            _disposed = true;
        }
        #endregion
    }
}
