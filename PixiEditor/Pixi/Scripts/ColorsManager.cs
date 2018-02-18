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

            public static Color ChangeColorBrightness(Color color, float correctionFactor)
            {
                float red = (float)color.R;
                float green = (float)color.G;
                float blue = (float)color.B;

                if (correctionFactor < 0)
                {
                    correctionFactor = 1 + correctionFactor;
                    red *= correctionFactor;
                    green *= correctionFactor;
                    blue *= correctionFactor;
                }
                else
                {
                    red = (255 - red) * correctionFactor + red;
                    green = (255 - green) * correctionFactor + green;
                    blue = (255 - blue) * correctionFactor + blue;
                }

                return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
            }
        }
    }
}
