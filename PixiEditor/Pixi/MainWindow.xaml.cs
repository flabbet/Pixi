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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
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
        public static Cursor activeCursor;
        public static ColorPicker firstColorPicker, secondColorPicker;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = System.Windows.WindowState.Maximized;
            pM.mainPanel = MainPanel;
            activeCursor = this.Cursor;
            firstColorPicker = FirstColorPicker;
            secondColorPicker = SecondColorPicker;
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

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.Source == FirstColorPicker)
            {
                ToolSettings.SetColorsToDraw(true);
            }
            else
            {
                ToolSettings.SetColorsToDraw(false);
            }
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Pen;
        }

       
        //Save button
       /* private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToolSettings.CreateSaveBitmap(MainPanel, @"D:\temp\test image.png");
        }
        */
        
        
    }
}
