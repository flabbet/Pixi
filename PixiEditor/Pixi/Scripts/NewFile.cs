using Pixi.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Pixi
{
    namespace IO
    {
        class NewFile
        {
            private ChildWindow inputPopup;
            private static TextBox sizeTextBox;
            private string sizeInputBoxContent;

            public NewFile()
            {
                CreateSizePopup();
            }

            private void CreateSizePopup()
            {
                MainWindow.newButton.IsEnabled = false;
                //creates popup
                inputPopup = new ChildWindow
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
                inputPopup.CloseButtonClicked += InputPopup_CloseButtonClicked;

                grid.Children.Add(message);
                grid.Children.Add(sizeTextBox);
                grid.Children.Add(button);
                inputPopup.Content = grid;
                MainWindow.popUpsArea.Children.Add(inputPopup);
                inputPopup.Show();
            }

            private void InputPopup_CloseButtonClicked(object sender, RoutedEventArgs e)
            {
                MainWindow.newButton.IsEnabled = true;
                MainWindow.popUpsArea.Children.RemoveAt(0);
            }

            private void SizeOkButton_Click(object sender, RoutedEventArgs e)
            {
                if (int.TryParse(sizeTextBox.Text, out int choosenSize))
                {
                    NewFileCreator(choosenSize);
                    inputPopup.Close();
                    MainWindow.popUpsArea.Children.RemoveAt(0);
                    MainWindow.newButton.IsEnabled = true;
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
                if(DrawArea.layers.Count > 0)
                {
                    DrawArea.Delete(DrawArea.image);
                }
                DrawArea layer = new DrawArea(size, DrawArea.mainPanel);
            }
        }
    }
}
