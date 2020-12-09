using CommonStyleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCoreLib;

namespace KeyConverterGUI
{
    public static class Constants
    {
        public const string KeyMapFileName = "keymap.json";
        public const string DetectProcessesFileName = "processes.json";

        public static readonly string AppDirectoryPath = AppInfo.GetAppPath();
        public static string IniFileName = AppDirectoryPath + @"\setting.ini";
    }
}
