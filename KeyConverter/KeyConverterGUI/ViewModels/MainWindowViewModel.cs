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
using CommonStyleLib.Views;
using KeyConverterGUI.Views;

namespace KeyConverterGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(WindowService windowService, MainWindowModel model) : base(windowService, model)
        {
            this.model = model;

            #region Initialize Properties
            ButtonText = model.ToReactivePropertyAsSynchronized(m => m.ButtonText);
            EnabledBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.EnabledBtEnabled);
            KeymappingBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeymappingBtEnabled);
            IsDetectMabinogi = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogi);
            IsDetectMabinogiEnabled = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogiEnabled);

            VersionText = $"v{CommonCoreLib.CommonFile.Version.GetVersion()}";
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
        public string VersionText { get; set; }
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
            model.EnabledBtEnabled = false;
            var keyboardModel = model.CreaKeyboardWindowModel();
            var vm = new KeyboardWindowViewModel(new WindowService(), keyboardModel);
            WindowManageService.ShowDialog<KeyboardWindow>(vm);
            keyboardModel.Dispose();
            model.EnabledBtEnabled = true;
            
            model.SaveKeyMap();
        }
        #endregion
    }
}
