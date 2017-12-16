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
using Pixi.FieldTools;

namespace Pixi
{
    namespace Settings
    {

        class ToolSettings
        {
            public static TextBox firstColorText;
            public static TextBox secondColorText;
            public static Rectangle firstColorRectangle;
            public static Rectangle secondColorRectangle;
            public static void SetColorsToDraw(bool firstColor)
            {
                try
                {
                    if (firstColor == true)
                    {
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(firstColorText.Text));
                        Tools.firstColor = mySolidColorBrush;
                        firstColorRectangle.Fill = mySolidColorBrush;
                        firstColorText.Text = null;
                    }
                    else
                    {
                        SolidColorBrush mySecondSolidColorBrush = new SolidColorBrush();
                        mySecondSolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(secondColorText.Text));
                        Tools.secondColor = mySecondSolidColorBrush;
                        secondColorRectangle.Fill = mySecondSolidColorBrush;
                        secondColorText.Text = null;
                    }
                }
                catch
                {
                    MessageBox.Show("Wrong Input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
