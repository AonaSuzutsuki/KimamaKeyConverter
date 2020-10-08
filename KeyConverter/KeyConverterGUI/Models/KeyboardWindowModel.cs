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
        private readonly Dictionary<OriginalKey, OriginalKey> keyMap;
        private ObservableDictionary<OriginalKey, string> label = new ObservableDictionary<OriginalKey, string>()
        {
            Default = " "
        };

        private SpecializedLowLevelKeyDetector interceptKeys;

        private bool keyboardIsEnabled = true;
        private Visibility settingWindowVisibility = Visibility.Collapsed;
        private string sourceKeyText;
        private string destKeyText;
        private OriginalKey srcKey;
        private OriginalKey destKey;
        #endregion

        #region Properties
        public ObservableDictionary<OriginalKey, string> Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        public bool KeyboardIsEnabled
        {
            get => keyboardIsEnabled;
            set => SetProperty(ref keyboardIsEnabled, value);
        }

        public Visibility SettingWindowVisibility
        {
            get => settingWindowVisibility;
            set => SetProperty(ref settingWindowVisibility, value);
        }

        public string SourceKeyText
        {
            get => sourceKeyText;
            set => SetProperty(ref sourceKeyText, value);
        }

        public string DestKeyText
        {
            get => destKeyText;
            set => SetProperty(ref destKeyText, value);
        }
        #endregion

        public KeyboardWindowModel(Dictionary<OriginalKey, OriginalKey> keyMap)
        {
            if (keyMap != null)
                this.keyMap = keyMap;
            else
                this.keyMap = new Dictionary<OriginalKey, OriginalKey>();
            Initialize();
        }

        private void Initialize()
        {
            foreach (var pair in keyMap)
                Label.Add(pair.Key, pair.Value.ToString());
        }

        public void OpenPopup(OriginalKey key)
        {
            srcKey = key;
            SourceKeyText = key.ToString();
            DestKeyText = "";
            destKey = OriginalKey.Unknown;

            interceptKeys = new SpecializedLowLevelKeyDetector();
            using (var process = Process.GetCurrentProcess())
            {
                interceptKeys.SpecificProcessId = process.Id;
            }
            interceptKeys.KeyDownEvent += Keyinput_KeyDownEvent;
            interceptKeys.Initialize();

            KeyboardIsEnabled = false;
            SettingWindowVisibility = Visibility.Visible;
        }

        private void Keyinput_KeyDownEvent(object sender, LowLevelKeyDetector.OriginalKeyEventArg e)
        {
            DestKeyText = e.Key.ToString();
            destKey = e.Key;
        }

        public void DestroyInput()
        {
            DestKeyText = OriginalKey.None.ToString();
            destKey = OriginalKey.None;
        }

        public void ApplyPopup()
        {
            if (destKey.Equals(OriginalKey.Unknown))
            {
                if (Label.ContainsKey(srcKey))
                    Label.Remove(srcKey);
                if (keyMap.ContainsKey(srcKey))
                    keyMap.Remove(srcKey);
            }
            else
            {
                if (Label.ContainsKey(srcKey))
                    Label[srcKey] = destKey.ToString();
                else
                    Label.Add(srcKey, destKey.ToString());

                if (keyMap.ContainsKey(srcKey))
                    keyMap[srcKey] = destKey;
                else
                    keyMap.Add(srcKey, destKey);
            }
            
            ClosePopup();
        }

        public void ClosePopup()
        {
            interceptKeys.UnHook();
            interceptKeys = null;

            KeyboardIsEnabled = true;
            SettingWindowVisibility = Visibility.Collapsed;
        }



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
                interceptKeys?.UnHook();
            }

            disposed = true;
        }
        #endregion
    }
}
