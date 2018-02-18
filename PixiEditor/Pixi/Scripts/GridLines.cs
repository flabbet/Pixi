using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Pixi
{
    namespace Settings
    {
        class GridLines
        {
            public DrawingVisual GridLinesDrawingVisual { get; }
            public GridLines()
            {
                var dv = new DrawingVisual();
                var dc = dv.RenderOpen();

                var w = DrawArea.areaSize;
                var h = w;
                var m = (DrawArea.image.Height / DrawArea.areaSize);
                var d = -0.5d; 

                var pen = new Pen(new SolidColorBrush(Color.FromArgb(63, 63, 63, 63)), 1.5d);

                pen.Freeze();

                for (var x = 1; x < w; x++)
                    dc.DrawLine(pen, new Point(x * m + d, 0), new Point(x * m + d, h * m));

                for (var y = 1; y < h; y++)
                    dc.DrawLine(pen, new Point(0, y * m + d), new Point(w * m, y * m + d));
                dc.Close();              
                GridLinesDrawingVisual = dv;
                VisualHost visualHost = new VisualHost { Visual = dv };
                DrawArea.mainPanel.Children.Add(visualHost);
            }

        }
    }
}
