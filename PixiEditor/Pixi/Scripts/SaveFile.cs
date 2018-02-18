using Microsoft.Win32;
using Pixi.FieldTools;
using Pixi.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace Pixi
{
    namespace IO
    {
        class SaveFile
        {
            private ChildWindow saveDialogPopup;
            private Label fileSize;
            private static byte fileSizeMultiplier = 1;
            public static string FilePath { get; private set; }

            public SaveFile(bool saveWithPopup = true)
            {
                if (saveWithPopup == true)
                {
                    CreateSaveDialog();
                }
                else
                {
                    SaveCanvasAsPng();
                }
            }


            private void CreateSaveDialog()
            {
                    MainWindow.saveAsButton.IsEnabled = false;
                    saveDialogPopup = new ChildWindow
                    {
                        WindowStartupLocation = Xceed.Wpf.Toolkit.WindowStartupLocation.Center,
                        Caption = "File properties",
                        Height = 150,
                        Width = 400,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        IsModal = true,
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
                        Maximum = 32,
                    };
                    slider.ValueChanged += SavePopUpSlider_ValueChanged;
                    fileSize = new Label()
                    {
                        FontSize = 20,
                        Height = 40,
                        Width = 400,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = (DrawArea.areaSize * slider.Value) + "x" + (DrawArea.areaSize * slider.Value),
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
                    saveDialogPopup.CloseButtonClicked += SaveDialogPopup_CloseButtonClicked;
                    saveDialogPopup.Content = grid;
                    grid.Children.Add(message);
                    grid.Children.Add(fileSize);
                    grid.Children.Add(slider);
                    grid.Children.Add(button);
                    MainWindow.popUpsArea.Children.Add(saveDialogPopup);
                    saveDialogPopup.Show();
                
            }

            private void SaveDialogPopup_CloseButtonClicked(object sender, RoutedEventArgs e)
            {
                MainWindow.saveAsButton.IsEnabled = true;
                MainWindow.popUpsArea.Children.RemoveAt(0);
            }

            private void SavePopUpSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                fileSizeMultiplier = (byte)(e.Source as Slider).Value;
                fileSize.Content = (DrawArea.areaSize * (byte)(e.Source as Slider).Value + "x" + (DrawArea.areaSize * (byte)(e.Source as Slider).Value));
            }

            private void SaveDialogButton_Click(object sender, RoutedEventArgs e)
            {
                saveDialogPopup.Close();
                MainWindow.popUpsArea.Children.RemoveAt(0);
                MainWindow.saveAsButton.IsEnabled = true;

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
                FilePath = saveLocationDialog.FileName;
                if (FilePath != "")
                {
                    MainWindow.saveButton.IsEnabled = true;
                    SaveCanvasAsPng();
                }
            }

            private static void SaveLocationDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e){}



            private void SaveCanvasAsPng()
            {
                DrawArea.activeLayer.LayerBitmap.SetPixel(Tools.lastX, Tools.lastY, Tools.lastColor);
                //DrawArea.canvasGridLines.GridLinesDrawingVisual.Opacity = 0;
                Rect bounds = VisualTreeHelper.GetDescendantBounds(DrawArea.mainPanel);
                double dpi = 96d;


                RenderTargetBitmap rtb = new RenderTargetBitmap(DrawArea.areaSize * fileSizeMultiplier, DrawArea.areaSize * fileSizeMultiplier, dpi, dpi, PixelFormats.Default);


                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(DrawArea.mainPanel);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(DrawArea.areaSize * fileSizeMultiplier, DrawArea.areaSize * fileSizeMultiplier)));
                }

                rtb.Render(dv);
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                try
                {
                    MemoryStream ms = new MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    File.WriteAllBytes(FilePath, ms.ToArray());
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //DrawArea.canvasGridLines.GridLinesDrawingVisual.Opacity = 1;
            }
        }
    }
}
