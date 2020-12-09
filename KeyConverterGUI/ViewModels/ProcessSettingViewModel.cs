using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProcessSettingViewModel : ViewModelBase
    {
        public ProcessSettingViewModel(ClearFocusWindowService windowService, ProcessSettingModel model) : base(windowService, model)
        {
            this.model = model;
            _clearFocusWindowService = windowService;

            ProcessItems = model.ProcessItems.ToReadOnlyReactiveCollection(m => m);
            ProcessSelectedItem = model.ToReactivePropertyAsSynchronized(m => m.ProcessSelectedItem);
            RemoveCurrentItemIsEnabled = model.ObserveProperty(m => m.CanRemove).ToReactiveProperty();

            ProcessItemsMouseDownCommand = new DelegateCommand<ProcessItemInfo>(ProcessItemsMouseDown);
            RemoveCurrentItemCommand = new DelegateCommand(RemoveCurrentItem);
            ContextMenuOpenedCommand = new DelegateCommand(ContextMenuOpened);
        }

        #region Fields

        private readonly ProcessSettingModel model;
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
            model.ProcessSelectedItem = null;
        }

        public void RemoveCurrentItem()
        {
            model.RemoveCurrentItem();
        }

        public void ContextMenuOpened()
        {
            if (model.ProcessSelectedItem == null)
                model.CanRemove = false;
            else
                model.CanRemove = model.ProcessSelectedItem.Type != ProcessItemType.Dummy;
        }
    }
}
