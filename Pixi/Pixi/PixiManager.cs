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
        public static Rectangle selectedRectangle;
        public static List<Rectangle> fields = new List<Rectangle>();

        public static void CreateDrawArea(int size)
        {
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
                clone.Height += 0.3f;
                clone.Width += 0.3f;
                clone.Fill = Brushes.Transparent;               
                clone.MouseEnter += Clone_MouseEnter;
                clone.MouseLeftButtonDown += Clone_MouseLeftButtonDown;
                clone.MouseRightButtonDown += Clone_MouseRightButtonDown;
                fields.Add(clone);
                timesDone++;
                mainPanel.Children.Add(clone);
            }
        }

        private static void Clone_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
            selectedRectangle.Fill = Brushes.Transparent;
        }

        private static void Clone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
            selectedRectangle.Fill = Brushes.BlueViolet;
        }

        public static void Clone_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                selectedRectangle.Fill = Brushes.BlueViolet;
            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                selectedRectangle = (Rectangle)(e.Source as FrameworkElement);
                selectedRectangle.Fill = Brushes.Transparent;
            }
            
        }
    }
}
