using CommonStyleLib.ViewModels;
using KeyConverterGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LowLevelKeyboardLib.KeyMap;
using System.Windows.Input;
using System.Collections.Concurrent;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using KeyConverterGUI.Views;
using System.Windows.Controls.Primitives;
using CommonStyleLib.Views;

namespace KeyConverterGUI.ViewModels
{
    public class KeyboardWindowViewModel : ViewModelBase
    {
        public KeyboardWindowViewModel(WindowService windowService, KeyboardWindowModel model) : base(windowService, model)
        {
            this.model = model;

            #region Initialize Properties
            Label = model.ToReactivePropertyAsSynchronized(m => m.Label);
            KeyboardIsEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeyboardIsEnabled);
            SettingWindowVisibility = model.ToReactivePropertyAsSynchronized(m => m.SettingWindowVisibility);
            SourceKeyText = model.ToReactivePropertyAsSynchronized(m => m.SourceKeyText);
            DestKeyText = model.ToReactivePropertyAsSynchronized(m => m.DestKeyText);
            #endregion

            #region Initialize Events
            KeyboardBtClicked = new DelegateCommand<OriginalKey?>(KeyboardBt_Clicked);
            DestroyInputButtonClicked = new DelegateCommand(DestroyInputButton_Clicked);
            OkPopupBtClicked = new DelegateCommand(OkPopupBt_Clicked);
            ClosePopupBtClicked = new DelegateCommand(ClosePopupBt_Clicked);
            #endregion
        }

        #region Fields
        private KeyboardWindowModel model;
        #endregion

        #region Properties
        public ReactiveProperty<bool> KeyboardIsEnabled { get; set; }
        public ReactiveProperty<Visibility> SettingWindowVisibility { get; set; }
        public ReactiveProperty<string> SourceKeyText { get; set; }
        public ReactiveProperty<string> DestKeyText { get; set; }
        #endregion

        #region Events Properties
        public ICommand KeyboardBtClicked { get; set; }
        public ICommand DestroyInputButtonClicked { get; set; }
        
        public ICommand OkPopupBtClicked { get; set; }
        public ICommand ClosePopupBtClicked { get; set; }
        #endregion

        #region Events Methods
        public void KeyboardBt_Clicked(OriginalKey? key)
        {
            if (key != null)
                model.OpenPopup(key.Value);
        }

        public void DestroyInputButton_Clicked()
        {
            model.DestroyInput();
        }

        public void OkPopupBt_Clicked()
        {
            model.ApplyPopup();
        }
        public void ClosePopupBt_Clicked()
        {
            model.ClosePopup();
        }
        #endregion


        #region Label Fields
        #endregion

        #region Label Properties
        public ReactiveProperty<ObservableDictionary<LowLevelKeyboardLib.KeyMap.OriginalKey, string>> Label
        {
            get;
            set;
        }
        #endregion
    }
}
