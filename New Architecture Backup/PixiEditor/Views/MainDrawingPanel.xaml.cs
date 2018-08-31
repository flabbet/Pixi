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
using Xceed.Wpf.Toolkit.Zoombox;

namespace PixiEditor.Views
{
    /// <summary>
    /// Interaction logic for MainDrawingPanel.xaml
    /// </summary>
    public partial class MainDrawingPanel : UserControl
    {
        public MainDrawingPanel()
        {
            InitializeComponent();
        }



        public bool CenterOnStart
        {
            get { return (bool)GetValue(CenterOnStartProperty); }
            set { SetValue(CenterOnStartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterOnStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterOnStartProperty =
            DependencyProperty.Register("CenterOnStart", typeof(bool), typeof(MainDrawingPanel), new PropertyMetadata(false));



        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(object), typeof(MainDrawingPanel), new PropertyMetadata(0));




        private void Zoombox_Loaded(object sender, RoutedEventArgs e)
        {
            if(CenterOnStart == true)
            {
                ((Zoombox)sender).CenterContent();
            }
        }
    }
}
