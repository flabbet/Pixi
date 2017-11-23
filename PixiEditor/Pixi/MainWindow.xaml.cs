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
using pM = Pixi.PixiManager;
using Pixi.FieldTools;

namespace Pixi
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            PixiManager.mainPanel = MainPanel;
            PixiManager.CreateDrawArea(64);
            Tools.OnStart();
        }

        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.FillBucket;
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Draw;
        }
    }
}
