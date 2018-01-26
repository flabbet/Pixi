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
using pM = Pixi.DrawArea;
using Pixi.FieldTools;
using Pixi.Settings;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using Pixi.IO;

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
        public static Canvas popUpsArea;
        public static Button saveAsButton;
        public static Button newButton;
        public static ColorPicker firstColorPicker, secondColorPicker;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = System.Windows.WindowState.Maximized;
            pM.mainPanel = MainPanel;
            activeCursor = this.Cursor;
            primaryGrid = MainGrid;
            transparentBackground = CanvasBackground;
            popUpsArea = PopUpsArea;
            newButton = NewFileButton;
            saveButton = SaveButton;
            saveAsButton = SaveAsButton;
            firstColorPicker = FirstColorPicker;
            secondColorPicker = SecondColorPicker;
            ColorsManager.firstColorRectangle = FirstColorRectangle;
            ColorsManager.secondColorRectangle = SecondColorRectangle;
            ColorsManager.firstColorRectangle.Fill = new SolidColorBrush(Tools.firstColor);
            ColorsManager.secondColorRectangle.Fill = new SolidColorBrush(Tools.secondColor);
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

        private void EarseButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Earse;
        }

        private void Zoombox_CurrentViewChanged(object sender, Xceed.Wpf.Toolkit.Zoombox.ZoomboxViewChangedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Zoom;
        }

        #endregion

        #region Menu buttons

        //creates size chooser popup 
        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            menuslistView.Visibility = Visibility.Collapsed;
            NewFile file = new NewFile();
        }

        //Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SaveButton)
            {
                menuslistView.Visibility = Visibility.Collapsed;
                SaveFile file = new SaveFile(false);
            }
            else
            {       
                menuslistView.Visibility = Visibility.Collapsed;
                SaveFile file = new SaveFile(true);
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
        #region Shortcuts
        private void PenTool_Shortcut(object sender, ExecutedRoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Pen;
        }

        private void FillTool_Shortcut(object sender, ExecutedRoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.FillBucket;
        }

        private void EarseTool_Shortcut(object sender, ExecutedRoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.Earse;
        }

        private void ColorPicker_Shortcut(object sender, ExecutedRoutedEventArgs e)
        {
            Tools.selectedTool = Tools.AvailableTools.ColorPicker;
        }
        #endregion

        #endregion

        #region Other
        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.Source == FirstColorPicker)
            {
                ColorsManager.SetColorsToDraw(true);
            }
            else
            {
                ColorsManager.SetColorsToDraw(false);
            }
        }
        #endregion

    }
}
