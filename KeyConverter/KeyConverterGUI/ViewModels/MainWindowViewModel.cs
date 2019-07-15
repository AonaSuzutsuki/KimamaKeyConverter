using CommonStyleLib.ViewModels;
using KeyConverterGUI.Models;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeyConverterGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(Window window, MainWindowModel model) : base(window, model)
        {
            this.model = model;

            #region Initialize Properties
            ButtonText = model.ToReactivePropertyAsSynchronized(m => m.ButtonText);
            EnabledBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.EnabledBtEnabled);
            KeymappingBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeymappingBtEnabled);
            IsDetectMabinogi = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogi);
            IsDetectMabinogiEnabled = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogiEnabled);
            #endregion

            #region Initialize Events
            EnabledBtClicked = new DelegateCommand(EnabledBt_Clicked);
            KeyboardMappingBtClicked = new DelegateCommand(KeyboardMappingBt_Clicked);
            #endregion
        }

        #region Fields
        private readonly MainWindowModel model;
        #endregion

        #region Properties
        public ReactiveProperty<string> ButtonText { get; set; }
        public ReactiveProperty<bool> EnabledBtEnabled { get; set; }
        public ReactiveProperty<bool> KeymappingBtEnabled { get; set; }
        public ReactiveProperty<bool> IsDetectMabinogi { get; set; }
        public ReactiveProperty<bool> IsDetectMabinogiEnabled { get; set; }
        #endregion

        #region Event Properties
        public ICommand EnabledBtClicked { get; set; }
        public ICommand KeyboardMappingBtClicked { get; set; }
        #endregion

        #region Event Methods
        protected override void MainWindow_Closing()
        {
            model.Dispose();
        }

        public void EnabledBt_Clicked()
        {
            model.EnabledOrDisabled();
        }

        public void KeyboardMappingBt_Clicked()
        {
            model.OpenKeyMapping();
        }
        #endregion
    }
}
