using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonStyleLib.File;
using CommonStyleLib.Models;
using KeyConverterGUI.Models.Data;
using LowLevelKeyboardLib.KeyMap;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace KeyConverterGUI.Models
{
    public enum ProcessItemType
    {
        Dummy,
        Item
    }

    public class ProcessItemInfo : BindableBase
    {
        private string _fullPath = string.Empty;

        public Dictionary<KeyEnum, KeyEnum> KeyMaps { get; set; }

        public Action<ProcessItemInfo> RemovoeItemAction { get; set; }

        public ProcessItemType Type { get; set; } = ProcessItemType.Item;

        public string FullPath
        {
            get => _fullPath;
            set
            {
                SetProperty(ref _fullPath, value);
                _fullPathChangedSubject.OnNext((this, value, Type));
                Type = ProcessItemType.Item;
            }
        }

        public ICommand SetFullPathCommand { get; set; }
        public ICommand FullPathGotFocusCommand { get; set; }
        public ICommand LostFocusCommand { get; set; }

        #region Event

        private readonly Subject<(ProcessItemInfo sender, string fullPath, ProcessItemType prevType)> _fullPathChangedSubject
            = new Subject<(ProcessItemInfo, string, ProcessItemType)>();
        public IObservable<(ProcessItemInfo sender, string fullPath, ProcessItemType prevType)> FullPathChanged => _fullPathChangedSubject;


        private readonly Subject<ProcessItemInfo> _fullPathGotFocuSubject = new Subject<ProcessItemInfo>();
        public IObservable<ProcessItemInfo> FullPathGotFocus => _fullPathGotFocuSubject;
        #endregion

        public ProcessItemInfo()
        {
            SetFullPathCommand = new DelegateCommand(SetFullPath);
            FullPathGotFocusCommand = new DelegateCommand(FullPathFocus);
            LostFocusCommand = new DelegateCommand(LostFocus);
        }

        public void SetFullPath()
        {
            var filePath = FileSelector.GetFilePath(CommonCoreLib.AppInfo.GetAppPath(), "All Files (*.*)|*.*", "",
                FileSelector.FileSelectorType.Read);
            FullPath = filePath ?? FullPath;
        }

        public void FullPathFocus()
        {
            _fullPathGotFocuSubject.OnNext(this);
        }

        public void LostFocus()
        {
            if (string.IsNullOrEmpty(FullPath))
                RemovoeItemAction?.Invoke(this);
        }
    }

    public class ProcessSettingModel : ModelBase
    {
        #region Fields

        private readonly string _jsonPath;

        private ProcessItemInfo _processSelectedItem;

        private bool _canRemove;

        #endregion

        #region Properties

        public ObservableCollection<ProcessItemInfo> ProcessItems { get; }

        public ProcessItemInfo ProcessSelectedItem
        {
            get => _processSelectedItem;
            set => SetProperty(ref _processSelectedItem, value);
        }

        public bool CanRemove
        {
            get => _canRemove;
            set => SetProperty(ref _canRemove, value);
        }

        #endregion

        public ProcessSettingModel(Dictionary<string, ProcessItem> processes)
        {
            if (processes.Count > 1)
            {
                var processesSet = processes.Where(x => x.Key != "Any");
                ProcessItems = new ObservableCollection<ProcessItemInfo>(from x in processesSet
                    select CreateProcessItemInfo(new ProcessItemInfo { FullPath = x.Key, KeyMaps = x.Value.KeyMaps}))
                {
                    CreateDummyProcessItemInfo()
                };
            }
            else
            {
                ProcessItems = new ObservableCollection<ProcessItemInfo>
                {
                    CreateDummyProcessItemInfo()
                };
            }
        }

        private ProcessItemInfo CreateProcessItemInfo(ProcessItemInfo processItemInfo)
        {
            processItemInfo.FullPathChanged.Subscribe(tuple =>
            {
                if (tuple.prevType == ProcessItemType.Dummy)
                    ProcessItems.Add(CreateDummyProcessItemInfo());
            });

            processItemInfo.FullPathGotFocus.Subscribe(item =>
            {
                ProcessSelectedItem = item;
            });

            processItemInfo.RemovoeItemAction = (info) =>
            {
                if (info.Type == ProcessItemType.Item)
                    ProcessItems.Remove(info);
            };

            return processItemInfo;
        }

        private ProcessItemInfo CreateDummyProcessItemInfo()
        {
            return CreateProcessItemInfo(new ProcessItemInfo
            {
                Type = ProcessItemType.Dummy
            });
        }

        public void RemoveCurrentItem()
        {
            var item = ProcessSelectedItem;
            if (item == null)
                return;

            if (item.Type != ProcessItemType.Dummy)
                ProcessItems.Remove(item);
        }

        public Dictionary<string, ProcessItem> GetProcesses()
        {
            var processesSet = (
                    from x in ProcessItems
                    where !string.IsNullOrEmpty(x.FullPath)
                    select new ProcessItem
                    {
                        FullPath = x.FullPath.ToLower(),
                        KeyMaps = x.KeyMaps
                    }
                ).ToDictionary(x => x.FullPath);

            return processesSet;
        }
    }
}
