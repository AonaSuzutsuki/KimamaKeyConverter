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
            #endregion

            #region Initialize Events
            EnabledBtClickCommand = new DelegateCommand(EnabledBtClick);
            KeyboardMappingBtClickCommand = new DelegateCommand(KeyboardMappingBtClick);
            ProcessSettingBtClickCommand = new DelegateCommand(ProcessSettingBtClick);
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
        public ICommand EnabledBtClickCommand { get; set; }
        public ICommand KeyboardMappingBtClickCommand { get; set; }
        public ICommand ProcessSettingBtClickCommand { get; set; }
        #endregion

        #region Event Methods
        protected override void MainWindow_Closing()
        {
            model.Dispose();
        }

        public void EnabledBtClick()
        {
            model.EnabledOrDisabled();
        }

        public void KeyboardMappingBtClick()
        {
            model.EnabledBtEnabled = false;
            var keyboardModel = model.CreaKeyboardWindowModel();
            var vm = new KeyboardWindowViewModel(new WindowService(), keyboardModel);
            WindowManageService.ShowDialog<KeyboardWindow>(vm);
            keyboardModel.Dispose();
            model.EnabledBtEnabled = true;
            
            model.SaveKeyMap();
        }

        public void ProcessSettingBtClick()
        {
            model.EnabledBtEnabled = false;
            var processModel = new ProcessSettingModel(Constants.IgnoreProcessesFileName);
            var vm = new ProcessSettingViewModel(new WindowService(), processModel);
            WindowManageService.ShowDialog<ProcessSetting>(vm);
            model.IgnoreProcesses = model.ConvertLowerHashSet(processModel.Save());
            model.EnabledBtEnabled = true;
        }
        #endregion
    }
}
