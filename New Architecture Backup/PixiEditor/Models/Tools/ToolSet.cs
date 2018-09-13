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
                    break;
                default:
                    break;
            }
            return writeableBitmap;
        }

        public void UpdateCoordinates(Coordinates cords)
        {
                _activeCoordinates = cords;
        }

        private void DrawPixel(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            canvas.SetPixel(pixelPosition.X, pixelPosition.Y, color);
        }

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
        /// This is frankenstein, meet my creation
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="coordinates"></param>
        /// <param name="color"></param>
        private async void LineAsync(Layer layer, Coordinates coordinates, Color color)
        {
            _toolIsExecuting = true;
            WriteableBitmap writeableBitmap = layer.LayerBitmap.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                layer.LayerBitmap.Clear();
                layer.LayerBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), writeableBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                layer.LayerBitmap.DrawLineBresenham(coordinates.X, coordinates.Y, _activeCoordinates.X, _activeCoordinates.Y, color);
                await Task.Delay(15);
            }
            writeableBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), layer.LayerBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
            _toolIsExecuting = false;
        }

        private async void RectangleAsync(Layer layer, Coordinates coordinates, Color color)
        {
            _toolIsExecuting = true;
            WriteableBitmap wb = layer.LayerBitmap.Clone();
            while (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                layer.LayerBitmap.Clear();
                layer.LayerBitmap.Blit(new Rect(new Size(layer.Width, layer.Height)), wb, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
                if (coordinates.X > _activeCoordinates.X && coordinates.Y > coordinates.Y)
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
                await Task.Delay(15);
            }
            wb.Blit(new Rect(new Size(layer.Width, layer.Height)), layer.LayerBitmap, new Rect(new Size(layer.Width, layer.Height)), WriteableBitmapExtensions.BlendMode.Additive);
            _toolIsExecuting = false;
        }

        private Color ColorPicker(Layer layer, Coordinates coordinates)
        {
            return layer.LayerBitmap.GetPixel(coordinates.X, coordinates.Y);
        }

        private void Earse(Layer layer, Coordinates coordinates)
        {
            layer.LayerBitmap.SetPixel(coordinates.X, coordinates.Y, Colors.Transparent);
        }
    }
}
