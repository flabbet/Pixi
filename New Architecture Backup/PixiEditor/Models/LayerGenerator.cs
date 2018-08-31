using PixiEditor.Models.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixiEditor.Models
{
    public static class LayerGenerator 
    {

        public static Layer GenerateLayer(int imageWidth, int imageHeight)
        {
            Image image = new Image();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
            image.Stretch = Stretch.Uniform;
            image.Width = imageWidth;
            image.Height = imageHeight;
            WriteableBitmap bitmap = GenerateBitmap(imageWidth, imageHeight);
            image.Source = bitmap;
            return new Layer(bitmap, image);
        }

        private static WriteableBitmap GenerateBitmap(int bitmapWidth, int imageHeight)
        {
            WriteableBitmap bitmap = BitmapFactory.New(bitmapWidth, imageHeight);
            bitmap.Clear(Colors.Transparent);
            return bitmap;
        }      
    }
}
