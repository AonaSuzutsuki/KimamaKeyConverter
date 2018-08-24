using CommonStyleLib.Models;
using InterceptKeyboardLib.KeyMap;
using KeyConverterGUI.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyConverterGUI.Models
{
    public class KeyboardWindowModel : ModelBase
    {

        #region Fields
        private ObservableDictionary<Key, string> label = new ObservableDictionary<Key, string>()
        {
            Default = " "
        };
        #endregion

        #region Properties
        public ObservableDictionary<Key, string> Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }
        #endregion

        public void Initialize()
        {
        }

        public void Test(Key key)
        {
            if (Label.ContainsKey(key))
                Label[key] = "X1";
            else
                Label.Add(key, "X");
        }
    }
}
