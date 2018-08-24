using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyConverterGUI.Views
{
    /// <summary>
    /// KeyboardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyboardWindow : Window
    {
        public KeyboardWindow()
        {
            InitializeComponent();

            var model = new Models.KeyboardWindowModel();
            var vm = new ViewModels.KeyboardWindowViewModel(this, model);
            DataContext = vm;
        }
    }
}
