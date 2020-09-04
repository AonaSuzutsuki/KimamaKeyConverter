using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using KeyConverterGUI.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace KeyConverterGUI.ViewModels
{
    public class ProcessSettingViewModel : ViewModelBase
    {
        public ProcessSettingViewModel(WindowService windowService, ProcessSettingModel model) : base(windowService, model)
        {
            ProcessItems = model.ProcessItems.ToReadOnlyReactiveCollection(m => m);
            ProcessesText = model.ToReactivePropertyAsSynchronized(m => m.ProcessesText);
        }

        #region Properties

        public ReadOnlyReactiveCollection<ProcessItemInfo> ProcessItems { get; set; }
        public ReactiveProperty<string> ProcessesText { get; set; }

        #endregion
    }
}
