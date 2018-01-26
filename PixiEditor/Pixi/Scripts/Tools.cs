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
using Pixi.Settings;
using System.Windows.Resources;
using System.Windows.Media.Imaging;

namespace Pixi
{
    namespace FieldTools
    {
        class Tools
        {
            public static AvailableTools selectedTool;                                          //selected tool variable
            private static Color pickedColor;                                                    //newColor that will be applied
            public static Color firstColor = Colors.Black, secondColor = Colors.Transparent;   //first and second newColor triggered to two mouse buttons   
            public enum AvailableTools
            {
                Pen = 0,
                FillBucket = 1,
                ColorPicker = 2,
                Earse = 3, 
                Zoom = 4,
            }

            //On application start
            public static void OnStart()
            {                            
                MainWindow.saveButton.IsEnabled = false;
            }

            //when draw area is created
            public static void OnDrawAreaCreated(Image image)
            {
                image.MouseMove += Image_MouseMove;
                image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                image.MouseRightButtonDown += Image_MouseRightButtonDown;
            }



            //Flood fill alghoritm
             public static void FloodFIll(int x,int y,Color newColor, Color colorToReplace)
            {
                if (DrawArea.drawAreaWBitmap.GetPixel(x, y).ToString() == "#00000000" && newColor == Colors.Transparent) return;

                     var stack = new Stack<Tuple<int, int>>();
                     stack.Push(Tuple.Create(x, y));

                     while (stack.Count > 0)
                     {
                         var point = stack.Pop();
                         if (point.Item1 < 0 || point.Item1 > DrawArea.areaSize-1) continue;
                         if (point.Item2 < 0 || point.Item2 > DrawArea.areaSize-1) continue;
                         if (DrawArea.drawAreaWBitmap.GetPixel(point.Item1, point.Item2) == newColor) continue;

                         if (DrawArea.drawAreaWBitmap.GetPixel(point.Item1, point.Item2) == colorToReplace)
                         {
                             DrawArea.drawAreaWBitmap.SetPixel(point.Item1, point.Item2, newColor);
                             stack.Push(Tuple.Create(point.Item1, point.Item2 - 1));
                             stack.Push(Tuple.Create(point.Item1 + 1, point.Item2));
                             stack.Push(Tuple.Create(point.Item1, point.Item2 + 1));
                             stack.Push(Tuple.Create(point.Item1 - 1, point.Item2));
                         }
                     }
             }            
            //draw tool
            public static void Draw(WriteableBitmap drawArea,Color color)
            {
                if (selectedTool == AvailableTools.Pen)
                {
                    Point point = Mouse.GetPosition(DrawArea.image);
                    int xRelativeToPixelSize = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                    int yRelativeToPixelSize = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));
                    
                   drawArea.SetPixel(xRelativeToPixelSize, yRelativeToPixelSize, color);                    
                }
            }

            public static void ColorPickerTool(bool setSecondColor)
            {
                if (selectedTool == AvailableTools.ColorPicker)
                {
                    Point point = Mouse.GetPosition(DrawArea.image);
                    int xRelativeToPixelSize = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                    int yRelativeToPixelSize = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));
                    if (setSecondColor == true)
                    {
                        secondColor = DrawArea.drawAreaWBitmap.GetPixel(xRelativeToPixelSize, yRelativeToPixelSize);
                        MainWindow.secondColorPicker.SelectedColor = secondColor;
                    }
                    else
                    {
                        firstColor = DrawArea.drawAreaWBitmap.GetPixel(xRelativeToPixelSize, yRelativeToPixelSize);                      
                        MainWindow.firstColorPicker.SelectedColor = firstColor;
                    }
                }
            }

            public static void EarseTool(WriteableBitmap fieldToOperateOn)
            {
                Point point = Mouse.GetPosition(DrawArea.image);
                int xRelativeToPixelSize = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                int yRelativeToPixelSize = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));
                fieldToOperateOn.SetPixel(xRelativeToPixelSize, yRelativeToPixelSize, Colors.Transparent);
            }

            //Check what tool is selected
            private static void CheckTool(bool senderIsRightMouseButton, bool onlyClicked = false)
            {             
                if(selectedTool == AvailableTools.Pen)
                {
                    Draw(DrawArea.drawAreaWBitmap, pickedColor);
                }
                else if(selectedTool == AvailableTools.FillBucket && onlyClicked == true)
                {
                    Point position = Mouse.GetPosition(DrawArea.image);
                    int xRelativeToPixelSize = (int)(position.X / (DrawArea.image.Height / DrawArea.areaSize));
                    int yRelativeToPixelSize = (int)(position.Y / (DrawArea.image.Height / DrawArea.areaSize));
                    Color colorToReplace = DrawArea.drawAreaWBitmap.GetPixel(xRelativeToPixelSize, yRelativeToPixelSize);
                    if (DrawArea.drawAreaWBitmap.GetPixel(xRelativeToPixelSize, yRelativeToPixelSize) == pickedColor) return;
                    FloodFIll(xRelativeToPixelSize,yRelativeToPixelSize, pickedColor, colorToReplace);
                }
                else if(selectedTool == AvailableTools.Earse)
                {
                    EarseTool(DrawArea.drawAreaWBitmap);
                }
                else if(selectedTool == AvailableTools.ColorPicker)
                {
                    ColorPickerTool(senderIsRightMouseButton);
                }

            }
            //Set newColor
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

            private static void Image_MouseMove(object sender, MouseEventArgs e)
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    SetColor(true);
                    CheckTool(false);
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    SetColor(false);
                    CheckTool(true);
                }
            }

            private static void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                SetColor(true);
                CheckTool(false, onlyClicked: true);
            }


            private static void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {
                SetColor(false);
                CheckTool(true, onlyClicked: true);
            }          
            #endregion
        }
    }
}
