using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.Models.Tools
{
    class ToolManager : INotifyPropertyChanged
    {

        private ToolType _activeTool;

        public ToolType ActiveTool
        {
            get { return _activeTool; }
            set
            {
                if (_activeTool != value)
                {
                    _activeTool = value;
                    RaisePropertyChanged("ActiveTool");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void RaisePropertyChanged(string property)
        {
            if(property != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
