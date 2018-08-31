using PixiEditor.Helpers;
using PixiEditor.Models;
using PixiEditor.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Zoombox;

namespace PixiEditor.ViewModels
{
    class ViewModelMain : ViewModelBase
    {        
        public RelayCommand SelectToolCommand { get; set; }
        public RelayCommand GenerateDrawAreaCommand { get; set; }

        private Layer _activeLayer;

        public Layer ActiveLayer
        {
            get { return _activeLayer; }
            set { _activeLayer = value; RaisePropertyChanged("ActiveLayer"); }
        }

        private bool _showDrawArea;

        public bool ShowDrawArea
        {
            get { return _showDrawArea; }
            set { _showDrawArea = value; RaisePropertyChanged("ShowDrawArea"); }
        }


        public ViewModelMain()
        {
            SelectToolCommand = new RelayCommand(RecognizeTool);
            GenerateDrawAreaCommand = new RelayCommand(GenerateDrawArea);
            ShowDrawArea = false;
        }

        private void RecognizeTool(object parameter)
        {
            
        }

        private void GenerateDrawArea(object parameter)
        {
            ActiveLayer = new Layer(16, 16);
            ShowDrawArea = true;          
        }
    }
}
