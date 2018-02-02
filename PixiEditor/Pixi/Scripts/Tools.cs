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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Pixi
{
    namespace FieldTools
    {
        class Tools
        {
            public static AvailableTools selectedTool;                                          //selected tool variable
            private static Color pickedColor;                                                    //newColor that will be applied
            public static int xCoords;
            public static int yCoords;
            private static bool mouseLeftClicked;
            private static WriteableBitmap wb = null;
            private static int clickedX, clickedY;
            public static Color firstColor = Colors.Black, secondColor = Colors.Transparent;   //first and second newColor triggered to two mouse buttons   
            public enum AvailableTools
            {
                Pen = 0,
                FillBucket = 1,
                ColorPicker = 2,
                Earse = 3, 
                Zoom = 4,
                Line = 5,
                Rectangle = 6,
                Triangle = 7,
                Circle = 8,
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
                image.MouseUp += Image_MouseUp;
                image.MouseLeave += Image_MouseLeave;
                image.MouseEnter += Image_MouseEnter;
            }


            //Flood fill alghoritm
            public static void FloodFIll(int x,int y,Color newColor, Color colorToReplace)
            {
                if (DrawArea.activeLayer.LayerBitmap.GetPixel(x, y).ToString() == "#00000000" && newColor == Colors.Transparent) return;

                     var stack = new Stack<Tuple<int, int>>();
                     stack.Push(Tuple.Create(x, y));

                     while (stack.Count > 0)
                     {
                         var point = stack.Pop();
                         if (point.Item1 < 0 || point.Item1 > DrawArea.areaSize-1) continue;
                         if (point.Item2 < 0 || point.Item2 > DrawArea.areaSize-1) continue;
                         if (DrawArea.activeLayer.LayerBitmap.GetPixel(point.Item1, point.Item2) == newColor) continue;

                         if (DrawArea.activeLayer.LayerBitmap.GetPixel(point.Item1, point.Item2) == colorToReplace)
                         {
                             DrawArea.activeLayer.LayerBitmap.SetPixel(point.Item1, point.Item2, newColor);
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
                   drawArea.SetPixel(xCoords, yCoords, color);                    
                }
            }

            public static void ColorPickerTool(bool setSecondColor)
            {
                if (selectedTool == AvailableTools.ColorPicker)
                {
                    if (setSecondColor == true)
                    {
                        secondColor = DrawArea.activeLayer.LayerBitmap.GetPixel(xCoords, yCoords);
                        MainWindow.secondColorPicker.SelectedColor = secondColor;
                    }
                    else
                    {
                        firstColor = DrawArea.activeLayer.LayerBitmap.GetPixel(xCoords, yCoords);                      
                        MainWindow.firstColorPicker.SelectedColor = firstColor;
                    }
                }
            }

            public static void EarseTool(WriteableBitmap fieldToOperateOn)
            {
                fieldToOperateOn.SetPixel(xCoords, yCoords, Colors.Transparent);
            }
            public static void LineTool()              
            {                     
                if(wb != null)
                {
                    DrawArea.activeLayer.LayerBitmap.Clear();
                    DrawArea.activeLayer.LayerBitmap.Blit(new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), wb, new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), WriteableBitmapExtensions.BlendMode.Additive);
                }
                wb = DrawArea.activeLayer.LayerBitmap.Clone();
                DrawArea.activeLayer.LayerBitmap.DrawLineBresenham(clickedX, clickedY, xCoords, yCoords, pickedColor);
            }

            public static void RectangleTool()
            {
                if (wb != null)
                {
                    DrawArea.activeLayer.LayerBitmap.Clear();
                    DrawArea.activeLayer.LayerBitmap.Blit(new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), wb, new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), WriteableBitmapExtensions.BlendMode.Additive);
                }
                wb = DrawArea.activeLayer.LayerBitmap.Clone();
                if (clickedX > xCoords && clickedY > yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawRectangle(xCoords, yCoords, clickedX, clickedY, pickedColor);
                }
                else if (clickedX < xCoords && clickedY < yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawRectangle(clickedX, clickedY, xCoords, yCoords, pickedColor);
                }
                else if(clickedY > yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawRectangle(clickedX, yCoords, xCoords, clickedY, pickedColor);

                }
                else
                {
                    DrawArea.activeLayer.LayerBitmap.DrawRectangle(xCoords, clickedY, clickedX, yCoords, pickedColor);
                }
            }

            public static void CircleTool()
            {
                if (wb != null)
                {
                    DrawArea.activeLayer.LayerBitmap.Clear();
                    DrawArea.activeLayer.LayerBitmap.Blit(new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), wb, new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), WriteableBitmapExtensions.BlendMode.Additive);
                }
                wb = DrawArea.activeLayer.LayerBitmap.Clone();
                if (clickedX > xCoords && clickedY > yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawEllipse(xCoords, yCoords, clickedX, clickedY, pickedColor);
                }
                else if (clickedX < xCoords && clickedY < yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawEllipse(clickedX, clickedY, xCoords, yCoords, pickedColor);
                }
                else if (clickedY > yCoords)
                {
                    DrawArea.activeLayer.LayerBitmap.DrawEllipse(clickedX, yCoords, xCoords, clickedY, pickedColor);

                }
                else
                {
                    DrawArea.activeLayer.LayerBitmap.DrawEllipse(xCoords, clickedY, clickedX, yCoords, pickedColor);
                }
            }

            //Check what tool is selected
            private static void CheckTool(bool senderIsRightMouseButton, bool onlyClicked = false)
            {             
                if(selectedTool == AvailableTools.Pen)
                {
                    Draw(DrawArea.activeLayer.LayerBitmap, pickedColor);
                }
                else if(selectedTool == AvailableTools.FillBucket && onlyClicked == true)
                {
                    Color colorToReplace = DrawArea.activeLayer.LayerBitmap.GetPixel(xCoords, yCoords);
                    if (DrawArea.activeLayer.LayerBitmap.GetPixel(xCoords, yCoords) == pickedColor) return;
                    FloodFIll(xCoords,yCoords, pickedColor, colorToReplace);
                }
                else if(selectedTool == AvailableTools.Earse)
                {
                    EarseTool(DrawArea.activeLayer.LayerBitmap);
                }
                else if(selectedTool == AvailableTools.ColorPicker)
                {
                    ColorPickerTool(senderIsRightMouseButton);
                }
                else if(selectedTool == AvailableTools.Line)
                {
                    LineTool();
                }
                else if(selectedTool == AvailableTools.Rectangle)
                {
                    RectangleTool();
                }
                else if (selectedTool == AvailableTools.Circle)
                {
                    CircleTool();
                }

            }            

            private static void HighlightField()
            {
            }

            #region events

            private static void Image_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    pickedColor = ColorsManager.SetColor(true,pickedColor);
                    CheckTool(false);
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    pickedColor = ColorsManager.SetColor(false, pickedColor);
                    CheckTool(true);
                }
                Point point = Mouse.GetPosition(DrawArea.image);
                xCoords = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                yCoords = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));
            }

            private static void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                Point point = Mouse.GetPosition(DrawArea.image);
                clickedX = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                clickedY = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));

                pickedColor = ColorsManager.SetColor(true, pickedColor);
                CheckTool(false, onlyClicked: true);
            }


            private static void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {
                Point point = Mouse.GetPosition(DrawArea.image);
                clickedX = (int)(point.X / (DrawArea.image.Height / DrawArea.areaSize));
                clickedY = (int)(point.Y / (DrawArea.image.Height / DrawArea.areaSize));

                pickedColor = ColorsManager.SetColor(false, pickedColor);
                CheckTool(true, onlyClicked: true);
            }

            private static void Image_MouseUp(object sender, MouseButtonEventArgs e)
            {
                wb = null;
                Actions.Action action = new Actions.Action(DrawArea.activeLayer);               
            }

            private static void Image_MouseLeave(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                {
                    Actions.Action action = new Actions.Action(DrawArea.activeLayer);
                    mouseLeftClicked = true;
                }
            }

            private static void Image_MouseEnter(object sender, MouseEventArgs e)
            {
                if(e.LeftButton != MouseButtonState.Pressed && e.RightButton != MouseButtonState.Pressed)
                {
                    wb = null;
                }
                if(e.LeftButton== MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                {
                    if (mouseLeftClicked == true)
                    {
                        Actions.Action.prevousBitmapsList.RemoveAt(Actions.Action.prevousBitmapsList.Count - 1);
                        mouseLeftClicked = false;
                    }
                }
            }
            #endregion
        }
    }
}
