using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public class ProcessItemInfo : BindableBase
    {
        private string fullPath = string.Empty;

        public string FullPath
        {
            get => fullPath;
            set => SetProperty(ref fullPath, value);
        }

        public ICommand SetFullPathCommand { get; set; }

        #region Event

        

        #endregion

        public ProcessItemInfo()
        {
            SetFullPathCommand = new DelegateCommand(SetFullPath);
        }

        public void SetFullPath()
        {
            var filePath = FileSelector.GetFilePath(CommonCoreLib.AppInfo.GetAppPath(), "All Files (*.*)|*.*", "",
                FileSelector.FileSelectorType.Read);
            FullPath = filePath ?? FullPath;
        }
    }

    public class ProcessSettingModel : ModelBase
    {
        #region Fields

        private HashSet<string> processesSet = new HashSet<string>();

        private ObservableCollection<ProcessItemInfo> processItems = new ObservableCollection<ProcessItemInfo>();
        private string processesText;
        private string jsonPath;

        #endregion

        #region Properties

        public ObservableCollection<ProcessItemInfo> ProcessItems
        {
            get => processItems;
            set => SetProperty(ref processItems, value);
        }

        public string ProcessesText
        {
            get => processesText;
            set => SetProperty(ref processesText, value);
        }

        #endregion

        public ProcessSettingModel(string jsonPath)
        {
            this.jsonPath = jsonPath;

            ProcessItems = new ObservableCollection<ProcessItemInfo>
            {
                CreateProcessItemInfo(new ProcessItemInfo
                {
                    FullPath = "test"
                }),
                CreateProcessItemInfo(new ProcessItemInfo
                {
                    FullPath = "test2"
                }),
                CreateProcessItemInfo(new ProcessItemInfo
                {
                    FullPath = ""
                })
            };

            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                processesSet = JsonConvert.DeserializeObject<HashSet<string>>(json);

                ProcessesText = string.Join("\n", processesSet);
            }
        }

        private ProcessItemInfo CreateProcessItemInfo(ProcessItemInfo processItemInfo)
        {

            return processItemInfo;
        }

        public HashSet<string> Save()
        {
            var lines = ProcessesText.Split('\n');
            processesSet.Clear();
            foreach (var line in lines)
            {
                if (!processesSet.Contains(line))
                    processesSet.Add(line);
            }

            var json = JsonConvert.SerializeObject(processesSet);
            File.WriteAllText(jsonPath, json);

            return processesSet;
        }
    }
}
