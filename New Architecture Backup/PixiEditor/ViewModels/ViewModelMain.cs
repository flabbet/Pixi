using PixiEditor.Helpers;
using PixiEditor.Models;
using PixiEditor.Models.Tools;
using PixiEditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.Zoombox;

namespace PixiEditor.ViewModels
{
    class ViewModelMain : ViewModelBase
    {
        public ObservableCollection<Layer> Layers { get; set; }

        public RelayCommand SelectToolCommand { get; set; } //Command that handles tool switching 
        public RelayCommand GenerateDrawAreaCommand { get; set; } //Command that generates draw area
        public RelayCommand MouseMoveOrClickCommand { get; set; } //Command that is used to draw

        private Layer _activeLayer;

        public Layer ActiveLayer //Active drawing layer
        {
            get { return _activeLayer; }
            set { _activeLayer = value; RaisePropertyChanged("ActiveLayer"); }
        }       

        private double _mouseXonCanvas;

        public double MouseXOnCanvas //Mouse X coordinate relative to canvas
        {
            get { return _mouseXonCanvas; }
            set { _mouseXonCanvas = value;  RaisePropertyChanged("MouseXonCanvas"); }
        }

        private double _mouseYonCanvas;

        public double MouseYOnCanvas //Mouse Y coordinate relative to canvas
        {
            get { return _mouseYonCanvas; }
            set { _mouseYonCanvas = value; RaisePropertyChanged("MouseYonCanvas"); }
        }


        private Color _primaryColor = Colors.White;

        public Color PrimaryColor //Primary color, hooked with left mouse button
        {
            get { return _primaryColor; }
            set { if (_primaryColor != value) { _primaryColor = value; RaisePropertyChanged("PrimaryColor"); } }
        }

        private Color _secondaryColor = Colors.Black;

        public Color SecondaryColor //Secondary color, hooked with right mouse button
        {
            get { return _secondaryColor; }
            set { if (_secondaryColor != value) { _secondaryColor = value; RaisePropertyChanged("SecondaryColor"); } }
        }

       

        private ToolType _selectedTool = ToolType.Pen;

        public ToolType SelectedTool
        {
            get { return _selectedTool; }
            set { if (_selectedTool != value) { _selectedTool = value; RaisePropertyChanged("SelectedTool"); } }
        }

        private ToolSet primaryToolSet;

        public ViewModelMain()
        {
            Layers = new ObservableCollection<Layer>();
            SelectToolCommand = new RelayCommand(RecognizeTool);
            GenerateDrawAreaCommand = new RelayCommand(GenerateDrawArea);
            MouseMoveOrClickCommand = new RelayCommand(MouseMoveOrClick);
            primaryToolSet = new ToolSet();
        }

        private void RecognizeTool(object parameter)
        {
            ToolType tool = (ToolType)Enum.Parse(typeof(ToolType), parameter.ToString());
            SelectedTool = tool;
        }

        /// <summary>
        /// Method connected with command, it executes tool "activity"
        /// </summary>
        /// <param name="parameter"></param>
        private void MouseMoveOrClick(object parameter)
        {
            Color color;
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                color = PrimaryColor;
            }
            else if(Mouse.RightButton == MouseButtonState.Pressed)
            {
                color = SecondaryColor;

            }
            else
            {
                return;
            }

            primaryToolSet.UpdateCoordinates(new Coordinates((int)MouseXOnCanvas, (int)MouseYOnCanvas));
            primaryToolSet.ExecuteTool(ActiveLayer, new Coordinates((int)MouseXOnCanvas, (int)MouseYOnCanvas), color, SelectedTool);
        }

        /// <summary>
        /// Generates new Layer and sets it as active one
        /// </summary>
        /// <param name="parameter"></param>
        private void GenerateDrawArea(object parameter)
        {
            Layers.Add(new Layer(16, 16));
            ActiveLayer = Layers[0];
        }
    }
}
