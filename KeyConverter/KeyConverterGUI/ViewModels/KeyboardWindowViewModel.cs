using CommonStyleLib.ViewModels;
using KeyConverterGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InterceptKeyboardLib.KeyMap;
using System.Windows.Input;
using System.Collections.Concurrent;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using KeyConverterGUI.Views;

namespace KeyConverterGUI.ViewModels
{
    public class KeyboardWindowViewModel : ViewModelBase
    {
        public KeyboardWindowViewModel(Window window, KeyboardWindowModel model) : base(window, model)
        {
            this.model = model;

            #region Initialize Properties
            Label = model.ToReactivePropertyAsSynchronized(m => m.Label);
            #endregion

            #region Initialize Events
            KeyboardBtClicked = new DelegateCommand<InterceptKeyboardLib.KeyMap.Key?>(KeyboardBt_Clicked);
            #endregion

            model.Initialize();
        }

        #region Fields
        private KeyboardWindowModel model;
        #endregion

        #region Properties

        #endregion

        #region Events Properties
        public ICommand KeyboardBtClicked { get; set; }
        #endregion

        #region Events Methods
        protected override void MainWindow_Loaded()
        {
        }

        public void KeyboardBt_Clicked(InterceptKeyboardLib.KeyMap.Key? key)
        {
            Console.WriteLine(key);
            if (key != null)
                model.Test(key.Value);
        }
        #endregion


        #region Label Fields
        #endregion

        #region Label Properties
        public ReactiveProperty<ObservableDictionary<InterceptKeyboardLib.KeyMap.Key, string>> Label
        {
            get;
            set;
        }
        #endregion
    }
}
