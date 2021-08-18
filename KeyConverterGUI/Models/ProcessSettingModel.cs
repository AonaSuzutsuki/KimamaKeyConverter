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
        private string fullPath = string.Empty;

        public Action<ProcessItemInfo> RemovoeItemAction { get; set; }

        public ProcessItemType Type { get; set; } = ProcessItemType.Item;

        public string FullPath
        {
            get => fullPath;
            set
            {
                SetProperty(ref fullPath, value);
                fullPathChangedSubject.OnNext((this, value, Type));
                Type = ProcessItemType.Item;
            }
        }

        public ICommand SetFullPathCommand { get; set; }
        public ICommand FullPathGotFocusCommand { get; set; }
        public ICommand LostFocusCommand { get; set; }

        #region Event

        private readonly Subject<(ProcessItemInfo sender, string fullPath, ProcessItemType prevType)> fullPathChangedSubject
            = new Subject<(ProcessItemInfo, string, ProcessItemType)>();
        public IObservable<(ProcessItemInfo sender, string fullPath, ProcessItemType prevType)> FullPathChanged => fullPathChangedSubject;


        private readonly Subject<ProcessItemInfo> fullPathGotFocuSubject = new Subject<ProcessItemInfo>();
        public IObservable<ProcessItemInfo> FullPathGotFocus => fullPathGotFocuSubject;
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
            fullPathGotFocuSubject.OnNext(this);
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

        private readonly string jsonPath;

        private ProcessItemInfo processSelectedItem;

        private bool canRemove;

        #endregion

        #region Properties

        public ObservableCollection<ProcessItemInfo> ProcessItems { get; }

        public ProcessItemInfo ProcessSelectedItem
        {
            get => processSelectedItem;
            set => SetProperty(ref processSelectedItem, value);
        }

        public bool CanRemove
        {
            get => canRemove;
            set => SetProperty(ref canRemove, value);
        }

        #endregion

        public ProcessSettingModel(string jsonPath)
        {
            this.jsonPath = jsonPath;

            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                var processesSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                ProcessItems = new ObservableCollection<ProcessItemInfo>(from x in processesSet
                    select CreateProcessItemInfo(new ProcessItemInfo {FullPath = x}))
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
                Type = ProcessItemType.Dummy,
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

        public HashSet<string> Save()
        {
            var processesSet = new HashSet<string>(from x in ProcessItems where !string.IsNullOrEmpty(x.FullPath) select x.FullPath);

            var json = JsonConvert.SerializeObject(processesSet);
            File.WriteAllText(jsonPath, json);

            return processesSet;
        }
    }
}
