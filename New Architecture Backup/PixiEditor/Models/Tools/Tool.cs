using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixiEditor.Models.Tools
{
    public static class Tool
    {
        public static void DrawPixel(WriteableBitmap canvas, Coordinates pixelPosition, Color color)
        {
            canvas.SetPixel(pixelPosition.X, pixelPosition.Y, color);
        }
    }
}
