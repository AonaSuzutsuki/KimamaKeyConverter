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
        public MainWindowViewModel(IWindowService windowService, MainWindowModel model) : base(windowService, model)
        {
            this._model = model;

            #region Initialize Properties
            ButtonText = model.ToReactivePropertyAsSynchronized(m => m.ButtonText).AddTo(_compositeDisposable);
            EnabledBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.EnabledBtEnabled).AddTo(_compositeDisposable);
            KeymappingBtEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeymappingBtEnabled).AddTo(_compositeDisposable);
            IsDetectMabinogi = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogi).AddTo(_compositeDisposable);
            IsDetectMabinogiEnabled = model.ToReactivePropertyAsSynchronized(m => m.IsDetectMabinogiEnabled).AddTo(_compositeDisposable);

            VersionText = $"v{CommonCoreLib.File.Version.GetVersion()}";
            #endregion

            #region Initialize Events
            EnabledBtClickCommand = new DelegateCommand(EnabledBtClick);
            KeyboardMappingBtClickCommand = new DelegateCommand(KeyboardMappingBtClick);
            ProcessSettingBtClickCommand = new DelegateCommand(ProcessSettingBtClick);
            #endregion
        }

        #region Fields

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly MainWindowModel _model;
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
            _model.Dispose();
        }

        public void EnabledBtClick()
        {
            _model.EnabledOrDisabled();
        }

        public void KeyboardMappingBtClick()
        {
            _model.EnabledBtEnabled = false;
            using (var keyboardModel = _model.CreateKeyboardWindowModel())
            {
                using var vm = new KeyboardWindowViewModel(new WindowService(), keyboardModel);
                WindowManageService.ShowDialog<KeyboardWindow>(vm);
            }
            _model.EnabledBtEnabled = true;
            
            _model.SaveKeyMap();
        }

        public void ProcessSettingBtClick()
        {
            _model.EnabledBtEnabled = false;
            var processModel = new ProcessSettingModel(Constants.DetectProcessesFileName);
            using var vm = new ProcessSettingViewModel(new ClearFocusWindowService(), processModel);
            WindowManageService.ShowDialog<ProcessSetting>(vm);
            _model.SetLowerHashSet(processModel.Save());
            _model.EnabledBtEnabled = true;
        }
        #endregion

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
