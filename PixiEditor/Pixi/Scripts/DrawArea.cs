using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pixi.Actions;
using Pixi.Core;
using Pixi.FieldTools;
using Pixi.IO;

namespace Pixi
{
    class DrawArea
    {
        public static Canvas mainPanel;
        public static int areaSize;
        public static Image image;
        public static List<Layer> layers = new List<Layer>();
        public static Layer activeLayer;

        public DrawArea(int size)
        {
            WriteableBitmap drawAreaWBitmap = BitmapFactory.New(size, size);
            image = new Image();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
            image.Stretch = Stretch.Uniform;
            image.Width = 816;
            image.Height = 816;
            image.Source = drawAreaWBitmap;
            mainPanel.Children.Add(image);
            drawAreaWBitmap.Clear(Colors.Transparent);
            areaSize = size;
            MainWindow.saveAsButton.IsEnabled = true;
            Tools.OnDrawAreaCreated(image);
            Layer layer = new Layer(drawAreaWBitmap, image);
            activeLayer = layer;
            layers.Add(layer);
            Actions.Action action = new Actions.Action(layer);
        }

        public static void Delete(Image drawArea)
        {
            layers.Clear();
            mainPanel.Children.Clear();
        }
    }
}
