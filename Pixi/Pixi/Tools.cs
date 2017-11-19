using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Pixi
{
    namespace Tools
    {
        class Tools
        {
            private int selectedTool;
            private Rectangle clickedRectangle = PixiManager.selectedRectangle;
            enum AvailableTools
            {
                Draw = 0,FillBucket = 1, 
            }
            void FillFields()
            {
                if(selectedTool == (int)AvailableTools.FillBucket)
                {
                    
                }
            }
        }
    }
}
