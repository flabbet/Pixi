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
using Pixi.FieldTools;


namespace Pixi
{
    class DrawArea
    {
        public static Canvas mainPanel;
        public static int areaSize;
        public static WriteableBitmap drawAreaWBitmap;
        public static Image image;
        public static List<Image> layers = new List<Image>();

        public static void Create(int size)
        {
            drawAreaWBitmap = BitmapFactory.New(size, size);
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
            layers.Add(image);
        }

        public static void Delete()
        {
            layers.Clear();
            mainPanel.Children.Clear();
        }       
    }
}
