using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Pixi.FieldTools;

namespace Pixi
{
    namespace Settings
    {
        class ColorsManager
        {
            public static Rectangle firstColorRectangle;
            public static Rectangle secondColorRectangle;

            //Set color to be applied
            public static void SetColorsToDraw(bool firstColor)
            {
                
                    if (firstColor == true)
                    {
                        Color myColor = new Color();
                        myColor = (Color)(ColorConverter.ConvertFromString(MainWindow.firstColorPicker.SelectedColorText));
                        Tools.firstColor = myColor;
                        firstColorRectangle.Fill = new SolidColorBrush(myColor);
                    }
                    else
                    {
                        Color mySecondColor = new Color();
                        mySecondColor = (Color)(ColorConverter.ConvertFromString(MainWindow.secondColorPicker.SelectedColorText));
                        Tools.secondColor = mySecondColor;
                        secondColorRectangle.Fill = new SolidColorBrush(mySecondColor);
                    }
                
              
            }

            public static Color SetColor(bool selectFirstColor, Color pickedColor)
            {
                if (selectFirstColor == true)
                {
                    return Tools.firstColor;
                }
                else
                {
                    return Tools.secondColor;
                }
            }
        }
    }
}
