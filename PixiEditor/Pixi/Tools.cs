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

namespace Pixi
{
    namespace FieldTools
    {
        class Tools
        {
            public static  AvailableTools selectedTool = AvailableTools.Draw;
            private static Rectangle selectedRectangle;
            public enum AvailableTools
            {
                Draw = 0, FillBucket = 1,
            }

            public static void OnStart()
            {
                MessageBox.Show("Every feature works perfectly with 64 x 64 or less canvas size, with bigger size some features can crash Pixi.", "Alpha build note", MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (Rectangle i in PixiManager.fields)
                {
                    i.MouseEnter += Field_MouseEnter;
                    i.MouseLeftButtonDown += Field_MouseLeftButtonDown;
                    i.MouseRightButtonDown += Field_MouseRightButtonDown;
                }
            }

            public static void FloodFIll(int x, int y, Brush color, Brush colorToReplace)
            {
                if (selectedTool == AvailableTools.FillBucket)
                {
                    if (x < 1 || x > PixiManager.drawAreaSize) return;
                    if (y < 1 || y > PixiManager.drawAreaSize) return;
                    try
                    {
                        PixiManager.FieldCords(x, y);
                    }
                    catch
                    {
                        return;
                    }
                    if (PixiManager.FieldCords(x, y).Fill != color)
                    {
                        if (PixiManager.FieldCords(x, y).Fill == colorToReplace)
                        {
                            PixiManager.FieldCords(x, y).Fill = color;
                            FloodFIll(x + 1, y, color, colorToReplace);
                            FloodFIll(x, y + 1, color, colorToReplace);
                            FloodFIll(x - 1, y, color, colorToReplace);
                            FloodFIll(x, y - 1, color, colorToReplace);
                        }
                    }
                }
            }

            public static void Draw(Rectangle fieldToColor, Brush color)
            {
                if (selectedTool == AvailableTools.Draw)
                {
                    if (fieldToColor.Fill != color)
                    {
                        fieldToColor.Fill = color;
                    }
                }
            }

            private static void CheckTool(Brush pickedColor, Brush secondColor)
            {
                if(selectedTool == AvailableTools.Draw)
                {
                    Draw(selectedRectangle, pickedColor);
                }
                else if(selectedTool == AvailableTools.FillBucket)
                {
                    FloodFIll(PixiManager.GetFieldX(selectedRectangle), PixiManager.GetFieldY(selectedRectangle), pickedColor, selectedRectangle.Fill);
                }
            }

#region events

            private static void Field_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                CheckTool(Brushes.Transparent, Brushes.Transparent);
            }

            private static void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                CheckTool(Brushes.Orange, Brushes.Olive);
            }

            private static void Field_MouseEnter(object sender, MouseEventArgs e)
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                    CheckTool(Brushes.Orange, Brushes.Olive);
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                    CheckTool(Brushes.Transparent, Brushes.Transparent);
                }
            }
#endregion
        }
    }
}
