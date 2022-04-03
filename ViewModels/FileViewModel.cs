using System;
using System.IO;
using WPF_TestApp.ViewModels.Base;

namespace WPF_TestApp.ViewModels
{
    public delegate void TreeviewItemSelectedHandler(string item, string name);

    public class FileViewModel : ViewModel
    {
        private FileInfo fileInfo;

        public static event TreeviewItemSelectedHandler OnItemSelected = delegate { };
        private bool _isSelected;
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (value == true)
                    OnItemSelected(this.Path, this.Name);
            }
        }
        private bool _isExpanded;
        public bool isExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        private string _Path;
        public string Path
        {
            get => fileInfo.FullName;
            set => Set(ref _Path, value);
        }

        private string _Name;
        public string Name
        {
            get => fileInfo.Name;
            set => Set(ref _Name, value);
        }

        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => fileInfo.CreationTime;
            set => Set(ref _CreationTime, value);
        }

        private long _SizeFile;
        public long SizeFile
        {
            get => fileInfo.Length;
            set => Set(ref _SizeFile, value);
        }

        public FileViewModel(string Path) => fileInfo = new FileInfo(Path);

    }

}
