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
using Pixi.Shortcuts;
using Microsoft.Win32;

namespace Pixi
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Cursor activeCursor;
        public static Border transparentBackground;
        public static Grid primaryGrid;
        public static ListView menuslistView;
        public static Button saveButton;
        public static ColorPicker firstColorPicker, secondColorPicker;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = System.Windows.WindowState.Maximized;
            pM.mainPanel = MainPanel;
            activeCursor = this.Cursor;
            primaryGrid = MainGrid;
            transparentBackground = CanvasBackground;
            saveButton = SaveButton;
            firstColorPicker = FirstColorPicker;
            secondColorPicker = SecondColorPicker;
            ToolSettings.firstColorRectangle = FirstColorRectangle;
            ToolSettings.secondColorRectangle = SecondColorRectangle;
            ToolSettings.firstColorRectangle.Fill = Tools.firstColor;
            ToolSettings.secondColorRectangle.Fill = Tools.secondColor;
            Tools.OnStart();
        }


        #region Tools
        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.FillBucket;
        }


        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Pen;
        }

        private void ColorPickerButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.ColorPicker;
        }

        #endregion

        #region Menu buttons

        //creates size chooser popup 
        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            menuslistView.Visibility = Visibility.Collapsed;
            ToolSettings.CreateSizePopup();
        }

        //Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SaveButton)
            {
                menuslistView.Visibility = Visibility.Collapsed;
                ToolSettings.SaveCanvasAsPng();
            }
            else
            {       
                menuslistView.Visibility = Visibility.Collapsed;
                ToolSettings.CreateSaveDialog();
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject dpobj = sender as DependencyObject;
            string name = (dpobj.GetValue(NameProperty) as string) + "ListView";
            ListView listViewToOpenClose;
            listViewToOpenClose = (ListView)FindName(name);
            menuslistView = listViewToOpenClose;
            if (listViewToOpenClose.Visibility == Visibility.Collapsed)
            {
                listViewToOpenClose.Visibility = Visibility.Visible;
            }
            else
            {
                listViewToOpenClose.Visibility = Visibility.Collapsed;
            }
        }



        #endregion

        #region Other
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
        #endregion

    }
}
