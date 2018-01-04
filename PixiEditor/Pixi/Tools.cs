using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Pixi.Shortcuts;
using System.Windows.Resources;

namespace Pixi
{
    namespace FieldTools
    {
        class Tools
        {
            public static  AvailableTools selectedTool = AvailableTools.Pen;                    //selected tool variable
            private static Brush pickedColor;                                                    //color that will be applied
            public static Brush firstColor = Brushes.Black, secondColor = Brushes.Transparent;    //first and second color triggered to two mouse buttons
            private static Rectangle mouseOnRectangle;
            private static Rectangle selectedRectangle;                                          //rectangle that is selected
            public enum AvailableTools
            {
                Pen = 0,
                FillBucket = 1,
                ColorPicker = 2,
            }

            //On application start
            public static void OnStart()
            {              
                MessageBox.Show("Every feature works with 64 x 64 or less canvas size, with bigger size some features may crash Pixi.", "Alpha build note", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.saveButton.IsEnabled = false;
            }

            //when draw area is created
            public static void OnDrawAreaCreated(Rectangle fieldToAdd)
            {
                fieldToAdd.MouseEnter += Field_MouseEnter;
                fieldToAdd.MouseLeftButtonDown += Field_MouseLeftButtonDown;
                fieldToAdd.MouseRightButtonDown += Field_MouseRightButtonDown;
                fieldToAdd.MouseLeave += I_MouseLeave;
            }

            //Flood fill alghoritm
            public static void FloodFIll(int x, int y, Brush color, Brush colorToReplace)
            {
                if (selectedTool == AvailableTools.FillBucket)
                {
                    if (x < 1 || x > PixiManager.drawAreaSize) return;
                    if (y < 1 || y > PixiManager.drawAreaSize) return;

                    if (PixiManager.FieldCords(x, y).Fill != color)
                    {
                        if (PixiManager.FieldCords(x, y).Fill == colorToReplace)
                        {
                            PixiManager.FieldCords(x, y).Fill = color;
                                FloodFIll(x,y-1, color, colorToReplace);
                                FloodFIll(x+1, y, color, colorToReplace);
                                FloodFIll(x, y+1, color, colorToReplace);
                                FloodFIll(x-1,y, color, colorToReplace);
                        }
                    }
                }
            }
            //draw tool
            public static void Draw(Rectangle fieldToColor, Brush color)
            {
                if (selectedTool == AvailableTools.Pen)
                {
                    if (fieldToColor.Fill != color)
                    {
                        fieldToColor.Fill = color;
                    }
                }
            }

            public static void ColorPickerTool(bool setSecondColor)
            {
                if (selectedTool == AvailableTools.ColorPicker)
                {
                    if (setSecondColor == true)
                    {
                        secondColor = selectedRectangle.Fill;
                        ToolSettings.secondColorRectangle.Fill = selectedRectangle.Fill;
                        SolidColorBrush fieldColor = (SolidColorBrush)selectedRectangle.Fill;
                        Color fieldColorColor = fieldColor.Color;
                        MainWindow.secondColorPicker.SelectedColor = fieldColorColor;
                    }
                    else
                    {
                        firstColor = selectedRectangle.Fill;
                        ToolSettings.firstColorRectangle.Fill = selectedRectangle.Fill;
                        SolidColorBrush fieldColor = (SolidColorBrush)selectedRectangle.Fill;
                        Color fieldColorColor = fieldColor.Color;
                        MainWindow.firstColorPicker.SelectedColor = fieldColorColor;
                    }
                }
            }
            //Check what tool is selected
            private static void CheckTool()
            {
                if(selectedTool == AvailableTools.Pen)
                {
                    Draw(selectedRectangle, pickedColor);
                }
                else if(selectedTool == AvailableTools.FillBucket)
                {
                    FloodFIll(PixiManager.GetFieldX(selectedRectangle), PixiManager.GetFieldY(selectedRectangle), pickedColor, selectedRectangle.Fill);
                }

            }
            //Set color
            public static void SetColor(bool selectFirstColor)
            {            
                if (selectFirstColor == true)
                {
                    pickedColor = firstColor;
                }
                else
                {
                    pickedColor = secondColor;
                }
            }

            #region events
           

            private static void Field_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {

                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                SetColor(false);
                ColorPickerTool(true);
                CheckTool();
            }

            private static void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                SetColor(true);
                ColorPickerTool(false);
                CheckTool();
            }

            private static void Field_MouseEnter(object sender, MouseEventArgs e)
            {               
                mouseOnRectangle = (Rectangle)(e.Source as FrameworkElement);
                mouseOnRectangle.Stroke = Brushes.Black;     
                mouseOnRectangle.StrokeThickness = 0.5f;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                    SetColor(true);
                    CheckTool();
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                    SetColor(false);
                    CheckTool();
                }
            }

            private static void I_MouseLeave(object sender, MouseEventArgs e)
            {
                mouseOnRectangle.StrokeThickness = 0;
            }
            #endregion
        }
    }
}
