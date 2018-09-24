using PixiEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PixiEditor.ViewModels
{
    class NewFileMenuViewModel : ViewModelBase
    {
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand DragMoveCommand { get; set; }

        public NewFileMenuViewModel()
        {
            OkCommand = new RelayCommand(OkButton);
            CloseCommand = new RelayCommand(CloseButton);
            DragMoveCommand = new RelayCommand(DragMove);
        }

        private void OkButton(object parameter)
        {
            ((Window)parameter).DialogResult = true;
            ((Window)parameter).Close();
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
    }
}
