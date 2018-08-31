using PixiEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PixiEditor.ViewModels
{
    class MenuButtonViewModel : ViewModelBase
    {
        public RelayCommand OpenListViewCommand { get; set; }
        private Visibility _listViewVisibility;

        public Visibility ListViewVisibility
        {
            get { return _listViewVisibility; }
            set { _listViewVisibility = value; RaisePropertyChanged("ListViewVisibility"); }
        }


        public MenuButtonViewModel()
        {
            OpenListViewCommand = new RelayCommand(OpenListView);
            ListViewVisibility = Visibility.Hidden;
        }

        private void OpenListView(object parameter)
        {
            ListViewVisibility = (ListViewVisibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
