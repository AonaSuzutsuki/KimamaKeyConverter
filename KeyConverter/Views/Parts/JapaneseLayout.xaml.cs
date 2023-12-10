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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyConverterGUI.Views.Parts
{
    /// <summary>
    /// JapaneseLayout.xaml の相互作用ロジック
    /// </summary>
    public partial class JapaneseLayout : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty KeyMapsProperty = DependencyProperty.Register(nameof(KeyMaps),
            typeof(ObservableDictionary<string, string>),
            typeof(JapaneseLayout),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public static readonly DependencyProperty KeyboardBtClickedProperty = DependencyProperty.Register(nameof(KeyboardBtClicked),
            typeof(ICommand),
            typeof(JapaneseLayout),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        public ObservableDictionary<string, string> KeyMaps
        {
            get => (ObservableDictionary<string, string>)GetValue(KeyMapsProperty);
            set => SetValue(KeyMapsProperty, value);
        }

        public ICommand KeyboardBtClicked
        {
            get => (ICommand)GetValue(KeyboardBtClickedProperty);
            set => SetValue(KeyboardBtClickedProperty, value);
        }


        public JapaneseLayout()
        {
            InitializeComponent();
        }
    }
}
