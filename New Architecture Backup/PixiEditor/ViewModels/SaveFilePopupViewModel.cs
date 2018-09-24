using Microsoft.Win32;
using PixiEditor.Helpers;
using PixiEditor.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PixiEditor.ViewModels
{
     class SaveFilePopupViewModel : ViewModelBase
    {
        public RelayCommand CloseButtonCommand { get; set; }
        public RelayCommand DragMoveCommand { get; set; }
        public RelayCommand ChoosePathCommand { get; set; }
        public RelayCommand OkCommand { get; set; }


        private string _pathButtonBorder = "#f08080";

        public string PathButtonBorder
        {
            get { return _pathButtonBorder; }
            set { if (_pathButtonBorder != value) { _pathButtonBorder = value; RaisePropertyChanged("PathButtonBorder"); } }
        }


        private bool _pathIsCorrect;

        public bool PathIsCorrect
        {
            get { return _pathIsCorrect; }
            set { if (_pathIsCorrect != value) { _pathIsCorrect = value; RaisePropertyChanged("PathIsCorrect"); } }
        }


        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { if (_filePath != value) { _filePath = value; RaisePropertyChanged("FilePath"); } }
        }

        public SaveFilePopupViewModel()
        {
            CloseButtonCommand = new RelayCommand(CloseButton);
            DragMoveCommand = new RelayCommand(DragMove);
            ChoosePathCommand = new RelayCommand(ChoosePath);
            OkCommand = new RelayCommand(OkButton, CanClickOk);
        }

        /// <summary>
        /// Command that handles Path choosing to save file
        /// </summary>
        /// <param name="parameter"></param>
        private void ChoosePath(object parameter)
        {
            SaveFileDialog path = new SaveFileDialog()
            {
                Title = "Export path",
                CheckPathExists = true,
                DefaultExt = "PNG Image (.png)|*.png",
                Filter = "PNG Image (.png)|*.png"
            };
            if(path.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(path.FileName) == false)
                {
                    PathButtonBorder = "#b8f080";
                    PathIsCorrect = true;
                    FilePath = path.FileName;
                }
                else
                {
                    PathButtonBorder = "#f08080";
                    PathIsCorrect = false;
                }
            }
        }

        private void CloseButton(object parameter)
        {
            ((Window)parameter).DialogResult = false;
            ((Window)parameter).Close();
        }

        private void DragMove(object parameter)
        {
            Window popup = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                popup.DragMove();
            }
        }

        private void OkButton(object parameter)
        {
            ((Window)parameter).DialogResult = true;
            ((Window)parameter).Close();
        }

        private bool CanClickOk(object property)
        {
            return PathIsCorrect == true;
        }
    }
}
