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
using CommonStyleLib.ExMessageBox.Views;
using CommonStyleLib.Views;

namespace KeyConverterGUI.Views
{
    /// <summary>
    /// KeyboardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyboardWindow : Window, IDisposable
    {

        private readonly KeyboardWindowModel model;

        public KeyboardWindow(Dictionary<OriginalKey, OriginalKey> keyMap)
        {
            InitializeComponent();

            model = new KeyboardWindowModel(keyMap);
            var vm = new ViewModels.KeyboardWindowViewModel(new WindowService(this), model);
            DataContext = vm;
        }


        #region IDisposable
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                model?.Dispose();
            }

            disposed = true;
        }
        #endregion
    }
}
