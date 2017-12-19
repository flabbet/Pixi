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
                    MessageBox.Show("Wrong Input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


           /* public static void CreateSaveBitmap(Canvas canvas, string filename)
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)canvas.Width, (int)canvas.Height,
                96d, 96d, PixelFormats.Pbgra32);
                // needed otherwise the image output is black
                canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
                canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));

                renderBitmap.Render(canvas);

                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                using (FileStream file = File.Create(filename))
                {
                    encoder.Save(file);
                }
                */
            } 
              
    }
} 

