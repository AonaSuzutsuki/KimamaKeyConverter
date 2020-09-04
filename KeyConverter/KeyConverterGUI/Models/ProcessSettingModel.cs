using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Models;
using Newtonsoft.Json;

namespace KeyConverterGUI.Models
{
    public class ProcessSettingModel : ModelBase
    {
        #region Fields

        private HashSet<string> processesSet = new HashSet<string>();

        private string processesText;
        private string jsonPath;

        #endregion

        #region Properties

        public string ProcessesText
        {
            get => processesText;
            set => SetProperty(ref processesText, value);
        }

        #endregion

        public ProcessSettingModel(string jsonPath)
        {
            this.jsonPath = jsonPath;

            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                processesSet = JsonConvert.DeserializeObject<HashSet<string>>(json);

                ProcessesText = string.Join("\n", processesSet);
            }
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
