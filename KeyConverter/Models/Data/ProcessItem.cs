using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LowLevelKeyboardLib.KeyMap;

namespace KeyConverterGUI.Models.Data
{
    public class ProcessItem
    {
        public string FullPath { get; set; }

        public Dictionary<KeyEnum, KeyEnum> KeyMaps { get; set; }
    }
}
