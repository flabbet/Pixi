using PixiEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public WriteableBitmap ExecuteTool(Layer layer, Coordinates startingCoords, Color color, ToolType tool)
        {
            WriteableBitmap writeableBitmap = layer.LayerBitmap;
            switch (tool)
            {
                case ToolType.Pen:
                    DrawPixel(writeableBitmap, startingCoords, color);
                    break;
                case ToolType.Bucket:
                    FloodFill(writeableBitmap, startingCoords, color);
                    break;
                case ToolType.Line:
                    if (_toolIsExecuting == false)
                    {
                        LineAsync(layer, startingCoords, color);
                    }
                    break;
                case ToolType.Circle:
                    if(_toolIsExecuting == false)
                    {
                        CircleAsync(layer, startingCoords, color);
                    }
                    break;
                case ToolType.Rectangle:
                    if(_toolIsExecuting == false)
                    {
                        RectangleAsync(layer, startingCoords, color);
                    }
                    break;              
                case ToolType.Earser:
                    Earse(layer, startingCoords);
                    break;
                case ToolType.Lighten:
                    if(Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        Lighten(layer.LayerBitmap, startingCoords);
                    }
                    else if(Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        Darken(layer.LayerBitmap, startingCoords);
                    }
                    break;
                default:
                    break;
            }
            return writeableBitmap;
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
        private void DrawPixel(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            canvas.SetPixel(pixelPosition.X, pixelPosition.Y, color);
        }

        /// <summary>
        /// Fills area with color (forest fire alghoritm)
        /// </summary>
        /// <param name="canvas">Bitmap to operate on</param>
        /// <param name="pixelPosition">Position of starting pixel</param>
        /// <param name="color">Fills area with this color</param>
        private void FloodFill(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            Color colorToReplace = canvas.GetPixel(pixelPosition.X, pixelPosition.Y);
            var stack = new Stack<Tuple<int, int>>();
            stack.Push(Tuple.Create(pixelPosition.X, pixelPosition.Y));

            while (stack.Count > 0)
            {
                var point = stack.Pop();
                if (point.Item1 < 0 || point.Item1 > canvas.Height - 1) continue;
                if (point.Item2 < 0 || point.Item2 > canvas.Width - 1) continue;
                if (canvas.GetPixel(point.Item1, point.Item2) == color) continue;

                if (canvas.GetPixel(point.Item1, point.Item2) == colorToReplace)
                {
                    canvas.SetPixel(point.Item1, point.Item2, color);
                    stack.Push(Tuple.Create(point.Item1, point.Item2 - 1));
                    stack.Push(Tuple.Create(point.Item1 + 1, point.Item2));
                    stack.Push(Tuple.Create(point.Item1, point.Item2 + 1));
                    stack.Push(Tuple.Create(point.Item1 - 1, point.Item2));
                }
            }
        }

        /// <summary>
        /// Draws line in canvas 
        /// </summary>
        /// <param name="layer">Layer to operate on</param>
        /// <param name="coordinates">Starting coordinates, usually click point</param>
        /// <param name="color">Does it really need a description?</param> 
        private async void LineAsync(Layer layer, Coordinates coordinates, Color color)
        {
            _toolIsExecuting = true;
            //clones bitmap before line
            WriteableBitmap writeableBitmap = layer.LayerBitmap.Clone();
            //While Mouse buttons are pressed, clears current bitmap, pastes cloned bitmap and draws line, on each iteration
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                layer.LayerBitmap.Clear();
                layer.LayerBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), writeableBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                layer.LayerBitmap.DrawLineBresenham(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
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
            //Basically does the same like rectangle method, but with different shape
            _toolIsExecuting = true;
            WriteableBitmap bitmap = layer.LayerBitmap.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                layer.LayerBitmap.Clear();
                layer.LayerBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), bitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                if (coordinates.X > _activeCoordinates.X && coordinates.Y > _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawEllipse(_activeCoordinates.X, _activeCoordinates.Y, coordinates.X, coordinates.Y, color);
                }
                else if (coordinates.X < _activeCoordinates.X && coordinates.Y < _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawEllipse(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                }
                else if (coordinates.Y > _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawEllipse(coordinates.X, _activeCoordinates.Y, _activeCoordinates.X, coordinates.Y, color);
                }
                else
                {
                    layer.LayerBitmap.DrawEllipse(_activeCoordinates.X, coordinates.Y, coordinates.X, _activeCoordinates.Y, color);
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
            _toolIsExecuting = true;
            WriteableBitmap wb = layer.LayerBitmap.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                //Two lines below are responsible for clearing last rectangle (on mouse move), to live show rectangle on bitmap
                layer.LayerBitmap.Clear();
                layer.LayerBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), wb, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                //Those if's are changing direction of rectangle, in other words: flips rectangle on X and Y axis when needed
                if (coordinates.X > _activeCoordinates.X && coordinates.Y > _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawRectangle(_activeCoordinates.X, _activeCoordinates.Y, coordinates.X, coordinates.Y, color);
                }
                else if (coordinates.X < _activeCoordinates.X && coordinates.Y < _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawRectangle(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                }
                else if (coordinates.Y > _activeCoordinates.Y)
                {
                    layer.LayerBitmap.DrawRectangle(coordinates.X, _activeCoordinates.Y, _activeCoordinates.X, coordinates.Y, color);
                }
                else
                {
                    layer.LayerBitmap.DrawRectangle(_activeCoordinates.X, coordinates.Y, coordinates.X, _activeCoordinates.Y, color);
                }
                await Task.Delay(_asyncDelay);
            }            
            _toolIsExecuting = false;
        }

        public static Color ColorPicker(Layer layer, Coordinates coordinates)
        {
            return layer.LayerBitmap.GetPixel(coordinates.X, coordinates.Y);
        }

        /// <summary>
        /// Earses (sets pixel color to transparent) pixel
        /// </summary>
        /// <param name="layer">Layer to operate on</param>
        /// <param name="coordinates">Pixel coordinates</param>
        private void Earse(Layer layer, Coordinates coordinates)
        {
            layer.LayerBitmap.SetPixel(coordinates.X, coordinates.Y, Colors.Transparent);
        }

        private void Lighten(WriteableBitmap bitmap, Coordinates coordinates)
        {
            Color newColor = ExColor.ChangeColorBrightness(bitmap.GetPixel(coordinates.X, coordinates.Y), 0.1f);
            bitmap.SetPixel(coordinates.X, coordinates.Y, newColor);
        }

        private void Darken(WriteableBitmap bitmap, Coordinates coordinates)
        {
            Color newColor = ExColor.ChangeColorBrightness(bitmap.GetPixel(coordinates.X, coordinates.Y), -0.06f);
            bitmap.SetPixel(coordinates.X, coordinates.Y, newColor);
        }
    }
}
