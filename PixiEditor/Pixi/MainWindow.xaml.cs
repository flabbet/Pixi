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
using Pixi.Settings;

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
            pM.mainPanel = MainPanel;
            pM.CreateDrawArea(32);
            ToolSettings.firstColorRectangle = FirstColorRectangle;
            ToolSettings.secondColorRectangle = SecondColorRectangle;
            ToolSettings.firstColorRectangle.Fill = Tools.firstColor;
            ToolSettings.secondColorRectangle.Fill = Tools.secondColor;
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

        private void ColorPickButton_Click(object sender, RoutedEventArgs e)
        {
            ToolSettings.firstColorText= FirstColorInputBox;
            ToolSettings.firstColorRectangle = FirstColorRectangle;
            if (e.Source == ColorPickButton)
            {
                ToolSettings.SetColorsToDraw(true);
            }
            else if(e.Source == SecondColorPickButton)
            {
                ToolSettings.secondColorText = SecondColorInputBox;
                ToolSettings.secondColorRectangle = SecondColorRectangle;
                ToolSettings.SetColorsToDraw(false);
            }
        }
    }
}
