using Pixi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Pixi
{
    namespace Actions
    {
        class Action
        {
            public static List<WriteableBitmap> prevousBitmapsList = new List<WriteableBitmap>();
            public static int UndoAmount { get; } = 25;
            public Action(Layer drawAreaBitmap)
            {
                WriteableBitmap wb = drawAreaBitmap.LayerBitmap.Clone();
                prevousBitmapsList.Add(wb);
            }

            public static void Undo()
            {
                if(prevousBitmapsList.Count > UndoAmount)
                {
                    prevousBitmapsList.RemoveAt(0);
                }
                DrawArea.activeLayer.LayerBitmap.Clear();
                DrawArea.activeLayer.LayerBitmap.Blit(new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), prevousBitmapsList[prevousBitmapsList.Count-2], new Rect(0, 0, DrawArea.areaSize, DrawArea.areaSize), WriteableBitmapExtensions.BlendMode.Additive);
                prevousBitmapsList.RemoveAt(prevousBitmapsList.Count - 1);
            }
        }
    }
}
