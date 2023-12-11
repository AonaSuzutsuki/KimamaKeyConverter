using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LowLevelKeyboardLib.KeyMap;

namespace KeyConverterGUI.Models.Data
{
    public enum KeyboardLayout
    {
        Us,
        Jis
    }

    public class SavedJson
    {
        public KeyboardLayout Layout { get; set; }

        public Dictionary<KeyEnum, KeyEnum> KeyMaps { get; set; }

        public string Version { get; set; } = "1.1";
    }
}
