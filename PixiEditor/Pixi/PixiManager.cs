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

//TODO: Ulepsz rozkładanie kafelków
//TODO: Dodaj paletę kolorów

namespace Pixi
{
    class PixiManager
    {
        public static Canvas mainPanel;
        public static Rectangle cloneCopy;
        public static int drawAreaSize;
        public static List<Rectangle> fields = new List<Rectangle>();

        public static void CreateDrawArea(int size)
        {
            drawAreaSize = size;
            int timesDone = 0;
            int toSpace = 0;
            for (int i = 0; i< size * size; i++)
            {                             
                Rectangle clone = new Rectangle();          
                clone.Name = "fieldCopyNumber" + i;
                cloneCopy = clone;
                clone.Height = mainPanel.Height / size;
                clone.Width = mainPanel.Width / size;
                if (timesDone == size)
                {
                    toSpace++;
                    timesDone = 0;
                }
                clone.SetValue(Canvas.LeftProperty, timesDone * clone.Width);
                clone.SetValue(Canvas.TopProperty, clone.Height * toSpace);
                clone.Height += 0.5f;
                clone.Width += 0.5f;
                clone.Fill = Brushes.Transparent;               
                fields.Add(clone);
                timesDone++;
                mainPanel.Children.Add(clone);
            }
        }

        public static Rectangle FieldCords(int x, int y)
        {
            try
            {
                return fields[(x - 1) + ((y - 1) * drawAreaSize)];
            }
            catch
            {
                MessageBox.Show("Propably incorrect coordinates", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }

        public static int GetFieldY(Rectangle field)
        {
            for (int i = 0; i < drawAreaSize; i++)
            {
                if(fields.FindIndex(x => x == field) > i * drawAreaSize && fields.FindIndex(x => x == field) < (i * drawAreaSize) + drawAreaSize)
                {
                    return ((i * drawAreaSize) + drawAreaSize) / drawAreaSize;
                }
                if (fields.FindIndex(x => x == field) < drawAreaSize)
                {
                    return 1;
                }
            }
            return -1;
        }

        public static int GetFieldX(Rectangle field)
        {
            int addOneToIndex = 0;
            for (int i = 0; i <= drawAreaSize; i++)
            {                    
                if(((GetFieldY(field)* drawAreaSize) - drawAreaSize) + addOneToIndex == fields.FindIndex(x=> x == field))
                {
                    if(addOneToIndex == 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return addOneToIndex + 1;
                    }
                }
                addOneToIndex++;                               
            }
            return -1;
        }
    }
}
