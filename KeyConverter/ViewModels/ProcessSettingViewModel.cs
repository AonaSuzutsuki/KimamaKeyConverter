using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonCoreLib.CommonLinq;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using KeyConverterGUI.Models;
using KeyConverterGUI.Views;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace KeyConverterGUI.ViewModels
{
    public class ProcessSettingViewModel : ViewModelWindowStyleBase, IDisposable
    {
        public ProcessSettingViewModel(ClearFocusWindowService windowService, ProcessSettingModel model) : base(windowService, model)
        {
            this._model = model;
            _clearFocusWindowService = windowService;

            ProcessItems = model.ProcessItems.ToReadOnlyReactiveCollection(m => m).AddTo(_compositeDisposable);
            ProcessSelectedItem = model.ToReactivePropertyAsSynchronized(m => m.ProcessSelectedItem).AddTo(_compositeDisposable);
            RemoveCurrentItemIsEnabled = model.ObserveProperty(m => m.CanRemove).ToReactiveProperty().AddTo(_compositeDisposable);

            ProcessItemsMouseDownCommand = new DelegateCommand<ProcessItemInfo>(ProcessItemsMouseDown);
            RemoveCurrentItemCommand = new DelegateCommand(RemoveCurrentItem);
            ContextMenuOpenedCommand = new DelegateCommand(ContextMenuOpened);
        }

        #region Fields

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly ProcessSettingModel _model;
        private readonly ClearFocusWindowService _clearFocusWindowService;

        #endregion

        #region Properties

        public ReadOnlyReactiveCollection<ProcessItemInfo> ProcessItems { get; set; }
        public ReactiveProperty<ProcessItemInfo> ProcessSelectedItem { get; set; }
        public ReactiveProperty<bool> RemoveCurrentItemIsEnabled { get; set; }

        #endregion

        #region Event Properties

        public ICommand ProcessItemsMouseDownCommand { get; set; }

        public ICommand RemoveCurrentItemCommand { get; set; }

        public ICommand ContextMenuOpenedCommand { get; set; }

        #endregion

        public void ProcessItemsMouseDown(ProcessItemInfo item)
        {
            _clearFocusWindowService.ClearFocus();
            _model.ProcessSelectedItem = null;
        }

        public void RemoveCurrentItem()
        {
            _model.RemoveCurrentItem();
        }

        public void ContextMenuOpened()
        {
            if (_model.ProcessSelectedItem == null)
                _model.CanRemove = false;
            else
                _model.CanRemove = _model.ProcessSelectedItem.Type != ProcessItemType.Dummy;
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
