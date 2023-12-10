using CommonStyleLib.ViewModels;
using KeyConverterGUI.Models;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LowLevelKeyboardLib.KeyMap;
using System.Windows.Input;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using KeyConverterGUI.Views;
using System.Windows.Controls.Primitives;
using CommonStyleLib.Views;

namespace KeyConverterGUI.ViewModels
{
    public class KeyboardWindowViewModel : ViewModelBase, IDisposable
    {
        public KeyboardWindowViewModel(IWindowService windowService, KeyboardWindowModel model) : base(windowService, model)
        {
            this._model = model;

            #region Initialize Properties

            model.Label.CollectionChanged += (sender, args) =>
            {
                void ChangeValue(IList objectList, string value = null)
                {
                    if (objectList.Count <= 0)
                        return;

                    var newItem = objectList[0];
                    if (!(newItem is KeyValuePair<KeyEnum, string> pair))
                        return;

                    var name = pair.Key.ToString();
                    if (Label.Value.ContainsKey(name))
                    {
                        Label.Value[name] = value ?? pair.Value;
                    }
                    else
                    {
                        Label.Value.Add(name, value ?? pair.Value);
                    }
                }

                if (args.NewItems == null)
                {
                    ChangeValue(args.OldItems, "");
                    return;
                }

                ChangeValue(args.NewItems);
            };

            Label = new ReactiveProperty<ObservableDictionary<string, string>>
            {
                Value = new ObservableDictionary<string, string>(
                    model.Label.ToDictionary(key => key.Key.ToString(), pair => pair.Value))
            };
            KeyboardIsEnabled = model.ToReactivePropertyAsSynchronized(m => m.KeyboardIsEnabled).AddTo(_compositeDisposable);
            SettingWindowVisibility = model.ToReactivePropertyAsSynchronized(m => m.SettingWindowVisibility).AddTo(_compositeDisposable);
            SourceKeyText = model.ToReactivePropertyAsSynchronized(m => m.SourceKeyText).AddTo(_compositeDisposable);
            DestKeyText = model.ToReactivePropertyAsSynchronized(m => m.DestKeyText).AddTo(_compositeDisposable);
            #endregion

            #region Initialize Events
            KeyboardBtClicked = new DelegateCommand<KeyEnum?>(KeyboardBt_Clicked);
            DestroyInputButtonClicked = new DelegateCommand(DestroyInputButton_Clicked);
            OkPopupBtClicked = new DelegateCommand(OkPopupBt_Clicked);
            ClosePopupBtClicked = new DelegateCommand(ClosePopupBt_Clicked);
            #endregion
        }

        #region Fields

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly KeyboardWindowModel _model;
        #endregion

        #region Properties
        public ReactiveProperty<bool> KeyboardIsEnabled { get; set; }
        public ReactiveProperty<Visibility> SettingWindowVisibility { get; set; }
        public ReactiveProperty<string> SourceKeyText { get; set; }
        public ReactiveProperty<string> DestKeyText { get; set; }
        #endregion

        #region Events Properties
        public ICommand KeyboardBtClicked { get; set; }
        public ICommand DestroyInputButtonClicked { get; set; }
        
        public ICommand OkPopupBtClicked { get; set; }
        public ICommand ClosePopupBtClicked { get; set; }
        #endregion

        #region Events Methods
        public void KeyboardBt_Clicked(KeyEnum? key)
        {
            if (key != null)
                _model.OpenPopup(key.Value);
        }

        public void DestroyInputButton_Clicked()
        {
            _model.DestroyInput();
        }

        public void OkPopupBt_Clicked()
        {
            _model.ApplyPopup();
        }
        public void ClosePopupBt_Clicked()
        {
            _model.ClosePopup();
        }
        #endregion


        #region Label Fields
        #endregion

        #region Label Properties
        public ReactiveProperty<ObservableDictionary<string, string>> Label
        {
            get;
            set;
        }
        #endregion

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
