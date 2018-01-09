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
        class FileMenu
        {
            private static TextBox sizeTextBox;
            private static ChildWindow inputPopup;
            private static ChildWindow saveDialogWindow;
            private static Label fileSize;
            private static string sizeInputBoxContent;

            private static string fileName;
            private static byte fileSizeMultiplier;

            #region NewFile
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
                MainWindow.popUpsArea.Children.Add(inputPopup);
                inputPopup.Show();
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

            //Creates draw area when recived data from popup           
            public static void NewFileCreator(int size)
            {
                MainWindow.transparentBackground.Visibility = Visibility.Visible;
                PixiManager.DeleteDrawArea();
                PixiManager.CreateDrawArea(size);
            }
            #endregion

            #region Save
            public static void CreateSaveDialog()
            {
                saveDialogWindow = new ChildWindow
                {
                    WindowStartupLocation = Xceed.Wpf.Toolkit.WindowStartupLocation.Center,
                    Caption = "File properties",
                    Height = 150,
                    Width = 400,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid grid = new Grid();
                Label message = new Label()
                {
                    FontSize = 15,
                    Height = 40,
                    Width = 400,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = "Set multiplier for file size",
                };
                Slider slider = new Slider()
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 120,
                    Height = 30,
                    Minimum = 1,
                    Maximum = 20,                    
                };
                slider.ValueChanged += SavePopUpSlider_ValueChanged;
                fileSize = new Label()
                {
                    FontSize = 20,
                    Height = 40,
                    Width = 400,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = (PixiManager.drawAreaSize * slider.Value) + "x" + (PixiManager.drawAreaSize * slider.Value),                   
                };
                Button button = new Button()
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Height = 40,
                    Width = 80,
                    Margin = new Thickness(0, 0, 10, 10),
                    Content = "Ok",
                };
                button.Click += SaveDialogButton_Click;
                saveDialogWindow.Content = grid;
                grid.Children.Add(message);
                grid.Children.Add(fileSize);
                grid.Children.Add(slider);
                grid.Children.Add(button);
                MainWindow.popUpsArea.Children.Add(saveDialogWindow);
                saveDialogWindow.Show();              
            }

            private static void SavePopUpSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                fileSizeMultiplier = (byte)(e.Source as Slider).Value;
                fileSize.Content = (PixiManager.drawAreaSize * (byte)(e.Source as Slider).Value + "x" + (PixiManager.drawAreaSize * (byte)(e.Source as Slider).Value));
            }

            private static void SaveDialogButton_Click(object sender, RoutedEventArgs e)
            {
                saveDialogWindow.Close();
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


                RenderTargetBitmap rtb = new RenderTargetBitmap(PixiManager.drawAreaSize * fileSizeMultiplier, PixiManager.drawAreaSize * fileSizeMultiplier, dpi, dpi, PixelFormats.Default);


                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(PixiManager.mainPanel);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(PixiManager.drawAreaSize * fileSizeMultiplier, PixiManager.drawAreaSize * fileSizeMultiplier)));
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
            #endregion


        }
    }
}
