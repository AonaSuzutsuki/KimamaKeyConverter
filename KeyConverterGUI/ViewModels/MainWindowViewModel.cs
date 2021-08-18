using CommonStyleLib.ViewModels;
using KeyConverterGUI.Models;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonStyleLib.Views;
using KeyConverterGUI.Views;

namespace KeyConverterGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        public MainWindowViewModel(WindowService windowService, MainWindowModel model) : base(windowService, model)
        {
            this.model = model;

            #region Initialize Properties
            ButtonText = model.ToReactivePropertyAsSynchronized(m => m.ButtonText).AddTo(compositeDisposable);
            EnabledBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.EnabledBtEnabled).AddTo(compositeDisposable);
            KeymappingBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeymappingBtEnabled).AddTo(compositeDisposable);
            IsDetectMabinogi = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogi).AddTo(compositeDisposable);
            IsDetectMabinogiEnabled = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogiEnabled).AddTo(compositeDisposable);

            VersionText = $"v{CommonCoreLib.File.Version.GetVersion()}";
            #endregion

            #region Initialize Events
            EnabledBtClickCommand = new DelegateCommand(EnabledBtClick);
            KeyboardMappingBtClickCommand = new DelegateCommand(KeyboardMappingBtClick);
            ProcessSettingBtClickCommand = new DelegateCommand(ProcessSettingBtClick);
            #endregion
        }

        #region Fields

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();
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
            using (var keyboardModel = model.CreateKeyboardWindowModel())
            {
                using var vm = new KeyboardWindowViewModel(new WindowService(), keyboardModel);
                WindowManageService.ShowDialog<KeyboardWindow>(vm);
            }
            model.EnabledBtEnabled = true;
            
            model.SaveKeyMap();
        }

        public void ProcessSettingBtClick()
        {
            model.EnabledBtEnabled = false;
            var processModel = new ProcessSettingModel(Constants.DetectProcessesFileName);
            using var vm = new ProcessSettingViewModel(new ClearFocusWindowService(), processModel);
            WindowManageService.ShowDialog<ProcessSetting>(vm);
            model.SetLowerHashSet(processModel.Save());
            model.EnabledBtEnabled = true;
        }
        #endregion

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}
