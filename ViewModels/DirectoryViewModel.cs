using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WPF_TestApp.ViewModels.Base;

namespace WPF_TestApp.ViewModels
{
    public delegate void TreeviewFolderSelectedHandler(string item,string name);

    public class DirectoryViewModel : ViewModel
    {
        private readonly DirectoryInfo directoryInfo;
        private bool _isExpanded;
        public bool isExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        public static event TreeviewItemSelectedHandler OnFolderSelected = delegate {};
        private bool _isSelected;
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (value == true)
                    OnFolderSelected(this.Path, this.Name);

            }
        }

        private string _Name;
        public string Name
        {
            get => directoryInfo.Name;
            set => Set(ref _Name, value);
        }

        private string _Path;
        public string Path
        {
            get => directoryInfo.FullName;
            set => Set(ref _Path, value);
        }

        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => directoryInfo.CreationTime;
            set => Set(ref _CreationTime, value);
        }

        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return directoryInfo.EnumerateDirectories().Select(dirInfo => new DirectoryViewModel(dirInfo.FullName));
                }
                catch (UnauthorizedAccessException)
                {

                }
                return Enumerable.Empty<DirectoryViewModel>();
            }
        }
        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    var files = directoryInfo.EnumerateFiles().Select(file => new FileViewModel(file.FullName));
                    return files;
                }
                catch (UnauthorizedAccessException)
                {

                }
                return Enumerable.Empty<FileViewModel>();
            }

        }
        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    return SubDirectories.Cast<object>().Concat(Files);
                }
                catch (UnauthorizedAccessException)
                {

                }
                return Enumerable.Empty<object>();
            }

        }

        public DirectoryViewModel(string Path) => directoryInfo = new DirectoryInfo(Path);

    }

}
