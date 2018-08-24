using CommonStyleLib.Models;
using KeyConverterGUI.Models.KeyManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyConverterGUI.Models
{
    public class MainWindowModel : ModelBase, IDisposable
    {
        #region Constants
        private const string DISABLED_TEXT = "Now Disabled";
        private const string ENABLED_TEXT = "Now Enabled";
        #endregion

        #region Fields
        private string buttonText = DISABLED_TEXT;

        InterceptKeys keyLogger;
        private bool isEnabled = false;
        #endregion

        #region Properties
        public string ButtonText
        {
            get => buttonText;
            set => SetProperty(ref buttonText, value);
        }
        #endregion

        public void EnabledOrDisabled()
        {
            if (!isEnabled)
            {
                keyLogger = new InterceptKeys();

                isEnabled = true;
                ButtonText = ENABLED_TEXT;
            }
            else
            {
                keyLogger.Dispose();

                isEnabled = false;
                ButtonText = DISABLED_TEXT;
            }
        }


        #region IDisposable
        public void Dispose()
        {
            keyLogger?.Dispose();
        }
        #endregion
    }
}
