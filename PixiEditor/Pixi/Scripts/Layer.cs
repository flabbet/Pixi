using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pixi
{
    namespace Core
    {
        class Layer
        {
            public WriteableBitmap LayerBitmap { get; set; }
            public Image LayerImage { get;}

            public Layer(WriteableBitmap layerWb, Image layerImage)
            {
                LayerBitmap = layerWb;
                LayerImage = layerImage;
            }

        }
    }
}
