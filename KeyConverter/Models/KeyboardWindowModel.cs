using CommonStyleLib.Models;
using LowLevelKeyboardLib.Input;
using LowLevelKeyboardLib.KeyMap;
using KeyConverterGUI.Models.InterceptKey;
using KeyConverterGUI.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonExtensionLib.Extensions;
using KeyConverterGUI.Models.Data;

namespace KeyConverterGUI.Models
{
    public class KeyboardWindowModel : ModelBase, IDisposable
    {

        #region Fields

        private Dictionary<KeyEnum, KeyEnum> _keyMap;
        private ObservableDictionary<KeyEnum, string> _label = new()
        {
            Default = " "
        };

        private readonly ObservableCollection<ProcessItem> _processes;
        private ProcessItem _processesSelectedItem;

        private SpecializedLowLevelKeyDetector _interceptKeys;

        private bool _keyboardIsEnabled = true;
        private Visibility _settingWindowVisibility = Visibility.Collapsed;
        private string _sourceKeyText;
        private string _destKeyText;
        private KeyEnum _srcKey;
        private KeyEnum _destKey;

        #endregion

        #region Properties

        public ObservableDictionary<KeyEnum, string> Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public ObservableCollection<ProcessItem> Processes => _processes;

        public ProcessItem ProcessesSelectedItem
        {
            get => _processesSelectedItem;
            set => SetProperty(ref _processesSelectedItem, value);
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

        public KeyboardWindowModel(Dictionary<string, ProcessItem> dict)
        {
            _processes = new ObservableCollection<ProcessItem>();
            _processes.AddRange(dict.Select(x => x.Value));

            ProcessesSelectedItem = _processes[0];

            ChangePreset(_processes[0]);
        }

        private void Initialize()
        {
            Label.Clear();
            foreach (var pair in _keyMap)
                Label.Add(pair.Key, pair.Value.ToString());
        }

        public void ChangePreset(ProcessItem processItem)
        {
            _keyMap = processItem.KeyMaps;

            Initialize();
        }

        public void OpenPopup(KeyEnum key)
        {
            _srcKey = key;
            SourceKeyText = key.ToString();
            DestKeyText = "";
            _destKey = KeyEnum.Unknown;

            _interceptKeys = new SpecializedLowLevelKeyDetector(new JapaneseKeyBoard());
            using (var process = Process.GetCurrentProcess())
            {
                _interceptKeys.SpecificProcessId = process.Id;
            }
            _interceptKeys.KeyDownEvent += Keyinput_KeyDownEvent;
            _interceptKeys.Initialize();

            KeyboardIsEnabled = false;
            SettingWindowVisibility = Visibility.Visible;
        }

        private void Keyinput_KeyDownEvent(object sender, LowLevelKeyDetector.KeyEnumEventArg e)
        {
            DestKeyText = e.Key.ToString();
            _destKey = e.Key;
        }

        public void DestroyInput()
        {
            DestKeyText = KeyEnum.None.ToString();
            _destKey = KeyEnum.None;
        }

        public void ApplyPopup()
        {
            if (_destKey.Equals(KeyEnum.Unknown))
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
            _interceptKeys.Dispose();
            _interceptKeys = null;

            KeyboardIsEnabled = true;
            SettingWindowVisibility = Visibility.Collapsed;
        }

        public Dictionary<string, ProcessItem> GetProcessItems()
        {
            return Processes.ToDictionary(x => x.FullPath);
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
