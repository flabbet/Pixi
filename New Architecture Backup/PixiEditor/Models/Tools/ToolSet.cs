﻿using PixiEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixiEditor.Models.Tools
{
    public class ToolSet
    {
        private Coordinates _activeCoordinates = new Coordinates();
        private bool _toolIsExecuting = false;
        private int _asyncDelay = 15;

        public Layer ExecuteTool(Layer layer, Coordinates startingCoords, Color color, ToolType tool)
        {
            Layer cLayer = layer;
            WriteableBitmap oldBitmap = layer.LayerBitmap.Clone();
            Image oldImage = layer.LayerImage;
            oldImage.Source = oldBitmap;
            switch (tool)
            {
                case ToolType.Pen:
                    cLayer.LayerBitmap = DrawPixel(cLayer.LayerBitmap, startingCoords, color);
                    break;
                case ToolType.Bucket:
                    cLayer.LayerBitmap = FloodFill(cLayer.LayerBitmap, startingCoords, color);
                    break;
                case ToolType.Line:
                    if (_toolIsExecuting == false)
                    {
                        LineAsync(cLayer, startingCoords, color);
                    }
                    break;
                case ToolType.Circle:
                    if(_toolIsExecuting == false)
                    {
                        CircleAsync(cLayer, startingCoords, color);
                    }
                    break;
                case ToolType.Rectangle:
                    if(_toolIsExecuting == false)
                    {
                        RectangleAsync(cLayer, startingCoords, color);
                    }
                    break;              
                case ToolType.Earser:
                    cLayer.LayerBitmap = Earse(cLayer, startingCoords);
                    break;
                case ToolType.Lighten:
                    if(Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        cLayer.LayerBitmap = Lighten(cLayer.LayerBitmap, startingCoords);
                    }
                    else if(Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        cLayer.LayerBitmap = Darken(cLayer.LayerBitmap, startingCoords);
                    }
                    break;
                default:
                    break;
            }
            UndoManager.RecordChanges("ActiveLayer", new Layer(oldBitmap, oldImage), cLayer, string.Format("{0} Tool.", tool.ToString()));
            return cLayer;
        }

        /// <summary>
        /// Updates coordinates in order to some tools work
        /// </summary>
        /// <param name="cords">Current coordinates</param>
        public void UpdateCoordinates(Coordinates cords)
        {
                _activeCoordinates = cords;
        }

        /// <summary>
        /// Fills pixel with choosen color
        /// </summary>
        /// <param name="canvas">Bitmap to operate on.</param>
        /// <param name="pixelPosition">Coordinates of pixel.</param>
        /// <param name="color">Color to be set.</param>
        private WriteableBitmap DrawPixel(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            WriteableBitmap bm = canvas;
            bm.SetPixel(pixelPosition.X, pixelPosition.Y, color);
            return bm;
        }

        /// <summary>
        /// Fills area with color (forest fire alghoritm)
        /// </summary>
        /// <param name="canvas">Bitmap to operate on</param>
        /// <param name="pixelPosition">Position of starting pixel</param>
        /// <param name="color">Fills area with this color</param>
        private WriteableBitmap FloodFill(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            WriteableBitmap bm = canvas;
            Color colorToReplace = bm.GetPixel(pixelPosition.X, pixelPosition.Y);
            var stack = new Stack<Tuple<int, int>>();
            stack.Push(Tuple.Create(pixelPosition.X, pixelPosition.Y));

            while (stack.Count > 0)
            {
                var point = stack.Pop();
                if (point.Item1 < 0 || point.Item1 > bm.Height - 1) continue;
                if (point.Item2 < 0 || point.Item2 > bm.Width - 1) continue;
                if (bm.GetPixel(point.Item1, point.Item2) == color) continue;

                if (bm.GetPixel(point.Item1, point.Item2) == colorToReplace)
                {
                    bm.SetPixel(point.Item1, point.Item2, color);
                    stack.Push(Tuple.Create(point.Item1, point.Item2 - 1));
                    stack.Push(Tuple.Create(point.Item1 + 1, point.Item2));
                    stack.Push(Tuple.Create(point.Item1, point.Item2 + 1));
                    stack.Push(Tuple.Create(point.Item1 - 1, point.Item2));
                }
            }
            return bm;
        }

        /// <summary>
        /// Draws line in canvas 
        /// </summary>
        /// <param name="layer">Layer to operate on</param>
        /// <param name="coordinates">Starting coordinates, usually click point</param>
        /// <param name="color">Does it really need a description?</param> 
        private async void LineAsync(Layer layer, Coordinates coordinates, Color color)
        {
            WriteableBitmap wb = layer.LayerBitmap;
            _toolIsExecuting = true;
            //clones bitmap before line
            WriteableBitmap writeableBitmap = wb.Clone();
            //While Mouse buttons are pressed, clears current bitmap, pastes cloned bitmap and draws line, on each iteration
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                wb.Clear();
                wb.Blit(new Rect(new Size(layer.Width, layer.Height)), writeableBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                wb.DrawLineBresenham(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                await Task.Delay(_asyncDelay);
            }           
            _toolIsExecuting = false;
        }

        /// <summary>
        /// Draws circle on bitmap.
        /// </summary>
        /// <param name="layer">Layer to operate on.</param>
        /// <param name="coordinates">Starting pixel coordinates.</param>
        /// <param name="color">Circle color.</param>
        private async void CircleAsync(Layer layer, Coordinates coordinates, Color color)
        {
            WriteableBitmap wb = layer.LayerBitmap;
            //Basically does the same like rectangle method, but with different shape
            _toolIsExecuting = true;
            WriteableBitmap bitmap = wb.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                wb.Clear();
                wb.Blit(new Rect(new Size(layer.Width, layer.Height)), bitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                if (coordinates.X > _activeCoordinates.X && coordinates.Y > _activeCoordinates.Y)
                {
                    wb.DrawEllipse(_activeCoordinates.X, _activeCoordinates.Y, coordinates.X, coordinates.Y, color);
                }
                else if (coordinates.X < _activeCoordinates.X && coordinates.Y < _activeCoordinates.Y)
                {
                    wb.DrawEllipse(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                }
                else if (coordinates.Y > _activeCoordinates.Y)
                {
                    wb.DrawEllipse(coordinates.X, _activeCoordinates.Y, _activeCoordinates.X, coordinates.Y, color);
                }
                else
                {
                    wb.DrawEllipse(_activeCoordinates.X, coordinates.Y, coordinates.X, _activeCoordinates.Y, color);
                }
                await Task.Delay(_asyncDelay);
            }
            _toolIsExecuting = false;
        }

        /// <summary>
        /// Draws rectangle on bitmap
        /// </summary>
        /// <param name="layer">Layer to operate on</param>
        /// <param name="coordinates">Starting pixel coordinate</param>
        /// <param name="color">Rectangle color</param>
        private async void RectangleAsync(Layer layer, Coordinates coordinates, Color color)
        {
            WriteableBitmap wb = layer.LayerBitmap;
            _toolIsExecuting = true;
            WriteableBitmap writeableBitmap = wb.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                //Two lines below are responsible for clearing last rectangle (on mouse move), to live show rectangle on bitmap
                wb.Clear();
                wb.Blit(new Rect(new Size(layer.Width, layer.Height)), writeableBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                //Those ifs are changing direction of rectangle, in other words: flips rectangle on X and Y axis when needed
                if (coordinates.X > _activeCoordinates.X && coordinates.Y > _activeCoordinates.Y)
                {
                    wb.DrawRectangle(_activeCoordinates.X, _activeCoordinates.Y, coordinates.X, coordinates.Y, color);
                }
                else if (coordinates.X < _activeCoordinates.X && coordinates.Y < _activeCoordinates.Y)
                {
                    wb.DrawRectangle(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                }
                else if (coordinates.Y > _activeCoordinates.Y)
                {
                    wb.DrawRectangle(coordinates.X, _activeCoordinates.Y, _activeCoordinates.X, coordinates.Y, color);
                }
                else
                {
                    wb.DrawRectangle(_activeCoordinates.X, coordinates.Y, coordinates.X, _activeCoordinates.Y, color);
                }
                await Task.Delay(_asyncDelay);
            }            
            _toolIsExecuting = false;
        }
        /// <summary>
        /// Returns color of pixel.
        /// </summary>
        /// <param name="layer">Layer in which bitmap with pixels are stored.</param>
        /// <param name="coordinates">Pixel coordinate.</param>
        /// <returns></returns>
        public static Color ColorPicker(Layer layer, Coordinates coordinates)
        {
            return layer.LayerBitmap.GetPixel(coordinates.X, coordinates.Y);
        }

        /// <summary>
        /// Earses (sets pixel color to transparent) pixel
        /// </summary>
        /// <param name="layer">Layer to operate on</param>
        /// <param name="coordinates">Pixel coordinates</param>
        private WriteableBitmap Earse(Layer layer, Coordinates coordinates)
        {
            WriteableBitmap wb = layer.LayerBitmap;
            wb.SetPixel(coordinates.X, coordinates.Y, Colors.Transparent);
            return wb;
        }
        /// <summary>
        /// Ligtens pixel color.
        /// </summary>
        /// <param name="bitmap">Bitmap to work on.</param>
        /// <param name="coordinates">Pixel coordinates.</param>
        /// <returns></returns>
        private WriteableBitmap Lighten(WriteableBitmap bitmap, Coordinates coordinates)
        {
            WriteableBitmap wb = bitmap;
            Color newColor = ExColor.ChangeColorBrightness(wb.GetPixel(coordinates.X, coordinates.Y), 0.1f);
            wb.SetPixel(coordinates.X, coordinates.Y, newColor);
            return wb;
        }
        /// <summary>
        /// Darkens pixel color.
        /// </summary>
        /// <param name="bitmap">Bitmap to work on.</param>
        /// <param name="coordinates">Pixel coordinates.</param>
        /// <returns></returns>
        private WriteableBitmap Darken(WriteableBitmap bitmap, Coordinates coordinates)
        {
            WriteableBitmap wb = bitmap;
            Color newColor = ExColor.ChangeColorBrightness(wb.GetPixel(coordinates.X, coordinates.Y), -0.06f);
            wb.SetPixel(coordinates.X, coordinates.Y, newColor);
            return wb;
        }
    }
}
