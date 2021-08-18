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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonStyleLib.Views;
using KeyConverterGUI.Models;
using KeyConverterGUI.ViewModels;

namespace KeyConverterGUI.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly MainWindowViewModel viewModel;
        private readonly MainWindowModel model;

        public MainWindow()
        {
            InitializeComponent();

            model = new MainWindowModel
            {
                ChangeBaseBackground = ChangeBaseBackground
            };
            viewModel = new MainWindowViewModel(new WindowService(this), model);
            DataContext = viewModel;
        }

        public void ChangeBaseBackground(SolidColorBrush color)
        {
            if (color == null)
                return;

            var storyboard = new Storyboard();
            var a = new ColorAnimation
            {
                To = color.Color,
                Duration = TimeSpan.FromSeconds(1)
            };
            Storyboard.SetTarget(a, BaseGrid);
            Storyboard.SetTargetProperty(a, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)"));
            storyboard.Children.Add(a);
            storyboard.Begin();
        }

        public void Dispose()
        {
            model?.Dispose();
            viewModel?.Dispose();
        }
    }
}
