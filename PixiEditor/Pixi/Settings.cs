using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using Microsoft.Win32;
using System.Windows.Shapes;
using Pixi.FieldTools;

namespace Pixi
{
    namespace Settings
    {

        class ToolSettings
        {
            public static Rectangle firstColorRectangle;
            public static Rectangle secondColorRectangle;

            //Set color to be applied
            public static void SetColorsToDraw(bool firstColor)
            {
                try
                {
                    if (firstColor == true)
                    {
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush = mySolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(MainWindow.firstColorPicker.SelectedColorText));
                        Tools.firstColor = mySolidColorBrush;
                        firstColorRectangle.Fill = mySolidColorBrush;
                    }
                    else
                    {
                        SolidColorBrush mySecondSolidColorBrush = new SolidColorBrush();
                        mySecondSolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(MainWindow.secondColorPicker.SelectedColorText));
                        Tools.secondColor = mySecondSolidColorBrush;
                        secondColorRectangle.Fill = mySecondSolidColorBrush;
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                
        }
        }
    }


