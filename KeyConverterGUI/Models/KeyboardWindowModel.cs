using CommonStyleLib.Models;
using LowLevelKeyboardLib.Input;
using LowLevelKeyboardLib.KeyMap;
using KeyConverterGUI.Models.InterceptKey;
using KeyConverterGUI.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyConverterGUI.Models
{
    public class KeyboardWindowModel : ModelBase, IDisposable
    {

        #region Fields
        private readonly Dictionary<OriginalKey, OriginalKey> _keyMap;
        private ObservableDictionary<OriginalKey, string> _label = new ObservableDictionary<OriginalKey, string>()
        {
            Default = " "
        };

        private SpecializedLowLevelKeyDetector _interceptKeys;

        private bool _keyboardIsEnabled = true;
        private Visibility _settingWindowVisibility = Visibility.Collapsed;
        private string _sourceKeyText;
        private string _destKeyText;
        private OriginalKey _srcKey;
        private OriginalKey _destKey;
        #endregion

        #region Properties
        public ObservableDictionary<OriginalKey, string> Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public bool KeyboardIsEnabled
        {
            get => _keyboardIsEnabled;
            set => SetProperty(ref _keyboardIsEnabled, value);
        }

        public Visibility SettingWindowVisibility
        {
            get => _settingWindowVisibility;
            set => SetProperty(ref _settingWindowVisibility, value);
        }

        public string SourceKeyText
        {
            get => _sourceKeyText;
            set => SetProperty(ref _sourceKeyText, value);
        }

        public string DestKeyText
        {
            get => _destKeyText;
            set => SetProperty(ref _destKeyText, value);
        }
        #endregion

        public KeyboardWindowModel(Dictionary<OriginalKey, OriginalKey> keyMap)
        {
            if (keyMap != null)
                this._keyMap = keyMap;
            else
                this._keyMap = new Dictionary<OriginalKey, OriginalKey>();
            Initialize();
        }

        private void Initialize()
        {
            foreach (var pair in _keyMap)
                Label.Add(pair.Key, pair.Value.ToString());
        }

        public void OpenPopup(OriginalKey key)
        {
            _srcKey = key;
            SourceKeyText = key.ToString();
            DestKeyText = "";
            _destKey = OriginalKey.Unknown;

            _interceptKeys = new SpecializedLowLevelKeyDetector();
            using (var process = Process.GetCurrentProcess())
            {
                _interceptKeys.SpecificProcessId = process.Id;
            }
            _interceptKeys.KeyDownEvent += Keyinput_KeyDownEvent;
            _interceptKeys.Initialize();

            KeyboardIsEnabled = false;
            SettingWindowVisibility = Visibility.Visible;
        }

        private void Keyinput_KeyDownEvent(object sender, LowLevelKeyDetector.OriginalKeyEventArg e)
        {
            DestKeyText = e.Key.ToString();
            _destKey = e.Key;
        }

        public void DestroyInput()
        {
            DestKeyText = OriginalKey.None.ToString();
            _destKey = OriginalKey.None;
        }

        public void ApplyPopup()
        {
            if (_destKey.Equals(OriginalKey.Unknown))
            {
                if (Label.ContainsKey(_srcKey))
                    Label.Remove(_srcKey);
                if (_keyMap.ContainsKey(_srcKey))
                    _keyMap.Remove(_srcKey);
            }
            else
            {
                if (Label.ContainsKey(_srcKey))
                    Label[_srcKey] = _destKey.ToString();
                else
                    Label.Add(_srcKey, _destKey.ToString());

                if (_keyMap.ContainsKey(_srcKey))
                    _keyMap[_srcKey] = _destKey;
                else
                    _keyMap.Add(_srcKey, _destKey);
            }
            
            ClosePopup();
        }

        public void ClosePopup()
        {
            _interceptKeys.UnHook();
            _interceptKeys = null;

            KeyboardIsEnabled = true;
            SettingWindowVisibility = Visibility.Collapsed;
        }



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
                _interceptKeys?.UnHook();
            }

            _disposed = true;
        }
        #endregion
    }
}
