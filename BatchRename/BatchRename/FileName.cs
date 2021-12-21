using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    class Filename : INotifyPropertyChanged
    {
        private string _currentName;
        private string _newName;
        private string _path;
        private string _result;
        public string CurrentName
        {
            get => _currentName;
            set
            {
                _currentName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentName"));
            }
        }
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewName"));
            }
        }
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path"));
            }
        }
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
