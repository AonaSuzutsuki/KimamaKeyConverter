using LowLevelKeyboardLib.KeyMap;
using KeyConverterGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyConverterGUI.Views
{
    /// <summary>
    /// KeyboardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyboardWindow : Window, IDisposable
    {

        private KeyboardWindowModel model;

        public KeyboardWindow(Dictionary<OriginalKey, OriginalKey> keyMap)
        {
            InitializeComponent();

            model = new KeyboardWindowModel(keyMap);
            var vm = new ViewModels.KeyboardWindowViewModel(this, model);
            DataContext = vm;
        }

        
        public void Dispose()
        {
            model?.Dispose();
        }
    }
}
