﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using KeyConverterGUI.Models;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace KeyConverterGUI.ViewModels
{
    public class ProcessSettingViewModel : ViewModelBase
    {
        public ProcessSettingViewModel(WindowService windowService, ProcessSettingModel model) : base(windowService, model)
        {
            this.model = model;

            ProcessItems = model.ProcessItems.ToReadOnlyReactiveCollection(m => m);
            ProcessSelectedItem = model.ObserveProperty(m => m.ProcessSelectedItem).ToReactiveProperty();

            ProcessItemsMouseDownCommand = new DelegateCommand<ProcessItemInfo>(ProcessItemsMouseDown);
        }

        #region Fields

        private readonly ProcessSettingModel model;

        #endregion

        #region Properties

        public ReadOnlyReactiveCollection<ProcessItemInfo> ProcessItems { get; set; }
        public ReactiveProperty<ProcessItemInfo> ProcessSelectedItem { get; set; }

        #endregion

        #region Event Properties

        public ICommand ProcessItemsMouseDownCommand { get; set; }

        #endregion

        public void ProcessItemsMouseDown(ProcessItemInfo item)
        {
            model.MouseDownOutside(item);
        }
    }
}
