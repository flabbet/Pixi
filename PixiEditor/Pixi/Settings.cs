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
    namespace Shortcuts
    {

        class ToolSettings
        {
            public static Rectangle firstColorRectangle;
            public static Rectangle secondColorRectangle;
            private static string sizeInputBoxContent;
            private static TextBox sizeTextBox;
            private static string fileName;
            private static ChildWindow inputPopup;

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
            public static void CreateSizePopup()
            {
                //creates popup
                inputPopup = new ChildWindow
                {
                    WindowStartupLocation = Xceed.Wpf.Toolkit.WindowStartupLocation.Center,
                    Caption = "File properties",
                    Height = 150,
                    Width = 400,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid grid = new Grid();
                //popup textbox
                
                sizeTextBox = new TextBox()
                {
                    Width = 100,
                    Height = 40,
                    FontSize = 30,
                    MaxLength = 4,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, 0, 10),
                };
                //message
                Label message = new Label()
                {
                    FontSize = 15,
                    Height = 40,
                    Width = 400,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = "Set size of fields (ex. 16, draw area size will be 16x16)",
                };
                //Ok button
                Button button = new Button()
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Height = 40,
                    Width = 80,
                    Margin = new Thickness(0, 0, 10, 10),
                    Content = "Ok",
                };
                button.Click += SizeOkButton_Click;
                sizeInputBoxContent = sizeTextBox.Text;

                grid.Children.Add(message);
                grid.Children.Add(sizeTextBox);
                grid.Children.Add(button);
                inputPopup.Content = grid;
                PixiManager.mainPanel.Children.Add(inputPopup);
                inputPopup.Show();
            }
            //Creates draw area when recived data from popup           
            public static void NewFileCreator(int size)
            {
                MainWindow.transparentBackground.Visibility = Visibility.Visible;
                PixiManager.DeleteDrawArea();
                PixiManager.CreateDrawArea(size);
            }

            private static void SizeOkButton_Click(object sender, RoutedEventArgs e)
            {
                if (int.TryParse(sizeTextBox.Text, out int choosenSize))
                {
                    NewFileCreator(choosenSize);
                    inputPopup.Close();
                }
                else
                {
                    sizeTextBox.BorderThickness = new Thickness(1);
                    sizeTextBox.BorderBrush = Brushes.Red;
                }
            }


             public static void CreateSaveDialog()
             {
                 SaveFileDialog saveLocationDialog = new SaveFileDialog
                {
                    Title = "Save location",
                    CheckPathExists = true,
                    AddExtension = true,
                    Filter = "All files (*.*)|*.*|PNG (*.png)|*.png",
                    DefaultExt = "png",
                    
                };
                saveLocationDialog.ShowDialog();
                saveLocationDialog.FileOk += SaveLocationDialog_FileOk;
                fileName = saveLocationDialog.FileName;
                if (fileName != "")
                {
                    MainWindow.saveButton.IsEnabled = true;
                    SaveCanvasAsPng();
                }
            }

            private static void SaveLocationDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
            {
            }


            public static void SaveCanvasAsPng()
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(PixiManager.mainPanel);
                double dpi = 96d;


                RenderTargetBitmap rtb = new RenderTargetBitmap(PixiManager.drawAreaSize, PixiManager.drawAreaSize, dpi, dpi, PixelFormats.Default);


                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(PixiManager.mainPanel);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(PixiManager.drawAreaSize, PixiManager.drawAreaSize)));
                }

                rtb.Render(dv);
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                try
                {
                    MemoryStream ms = new MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    File.WriteAllBytes(fileName, ms.ToArray());
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        }
    }


