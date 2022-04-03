using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF_TestApp.Infrastructure.Commands.Base;
using WPF_TestApp.ViewModels.Base;

namespace WPF_TestApp.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Заголовок окна
        private string _Title = "Проводник";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion
        #region Прогресс бар //не задействован
        private int _ProgressBar = 1;
        public int ProgressBar
        {
            get => _ProgressBar;
            set => Set(ref _ProgressBar, value);
        }
        #endregion
        #region Все необходимое
        private string _Hex;
        public string Hex
        {
            get => _Hex;
            set => Set(ref _Hex, value);
        }

        private string _PathFolder;
        public string PathFolder
        {
            get => _PathFolder;
            set => Set(ref _PathFolder, value);
        }
        private string _PathItem;
        public string PathItem
        {
            get => _PathItem;
            set => Set(ref _PathItem, value);
        }

        private string _NameItem;
        public string NameItem
        {
            get => _NameItem;
            set => Set(ref _NameItem, value);
        }
        private string _NameFolder;
        public string NameFolder
        {
            get => _NameFolder;
            set => Set(ref _NameFolder, value);
        }

        private object _Content;
        public object Content
        {
            get => _Content;
            set => Set(ref _Content, value);
        }

        private string _ImgVisibl = "Hidden";
        public string ImgVisibl
        {
            get => _ImgVisibl;
            set => Set(ref _ImgVisibl, value);
        }

        private string _HexVisibl = "Hidden";
        public string HexVisibl
        {
            get => _HexVisibl;
            set => Set(ref _HexVisibl, value);
        }

        private string _TxtVisibl = "Hidden";
        public string TxtVisibl
        {
            get => _TxtVisibl;
            set => Set(ref _TxtVisibl, value);
        }

        private DriveInfo[] _Drives = DriveInfo.GetDrives();
        public DriveInfo[] Drives
        {
            get => _Drives;
            set => Set(ref _Drives, value);
        }

        private string _SelectDrives;
        public string SelectDrives
        {
            get => _SelectDrives;
            set => Set(ref _SelectDrives, value);
        }

        private ObservableCollection<DirectoryViewModel> _HDD;
        public ObservableCollection<DirectoryViewModel> HDD
        {
            get => _HDD;
            set => Set(ref _HDD, value);
        }
        #endregion
        #region Команды

        public ICommand CloseAplicationCommand { get; }
        private bool CanCloseAplicationCommandExecute(object p) => true;
        private void OnCloseAplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        public ICommand CreateFolderCommand { get; }
        private bool CanCreateFolderCommandExecute(object p) => true;
        private void OnCreateFolderCommandExecuted(object p)
        {
            CreateFolder(PathFolder, "");
        }


        public ICommand DeleteItemCommand { get; }
        private bool CanDeleteItemCommandCommandExecute(object p) => true;
        private void OnDeleteItemCommandCommandExecuted(object p)
        {
            DeleteItem(PathItem, NameItem);
        }

        public ICommand DeleteFolderCommand { get; }
        private bool CanDeleteFolderCommandExecute(object p) => true;
        private void OnDeleteFolderCommandExecuted(object p)
        {
            DeleteFolder(PathFolder, NameFolder);
        }
        #endregion
        public MainWindowViewModel()
        {
            #region Команды
            CloseAplicationCommand = new ActionCommand(OnCloseAplicationCommandExecuted, CanCloseAplicationCommandExecute); //выход
            CreateFolderCommand = new ActionCommand(OnCreateFolderCommandExecuted, CanCreateFolderCommandExecute); //создание папки
            DeleteFolderCommand = new ActionCommand(OnDeleteFolderCommandExecuted, CanDeleteFolderCommandExecute); //удаление папки
            DeleteItemCommand = new ActionCommand(OnDeleteItemCommandCommandExecuted, CanDeleteItemCommandCommandExecute); //удаление элемента
            #endregion
            HDD = new ObservableCollection<DirectoryViewModel>() { };
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                HDD.Add(new DirectoryViewModel(drive.Name));
            }
            FileViewModel.OnItemSelected += FileViewModel_OnItemSelected;
            DirectoryViewModel.OnFolderSelected += DirectoryViewModel_OnFolderSelected;
        }
        private void FileViewModel_OnItemSelected(string item, string name)//инфо о файле
        {
            PathItem = item;
            NameItem = name;
            Data(PathItem);
        }
        private void DirectoryViewModel_OnFolderSelected(string item, string name)//инфо каталога
        {
            PathFolder = item;
            NameFolder = name;
        }
        public string GetHex(string path) //получаем hex
        {
            StringBuilder sb = new StringBuilder();
            byte[] readText = File.ReadAllBytes(path);
            foreach (byte s in readText)
            {
                sb.Append(s.ToString("X2"));
            }
            return sb.ToString();
        }
        public List<string> Hex16(string hex) //разбиваем hex по 16
        {
            var list = new List<string>();
            StringBuilder sb = new StringBuilder();
            var counter = 0;
            const int SymPerLine = 16;
            foreach (var ch in hex)
            {
                if (counter == SymPerLine)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                    counter = 0;
                }
                sb.Append(ch.ToString());
                counter++;
            }
            return list;
        }
        public void CreateFolder(string path, string name)  //создание каталога
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (name != "")
            {
                Directory.CreateDirectory($"{path}/{name}");
            }
            else
            {
                Directory.CreateDirectory($"{path}/Новая папка");
            }
        }
        public void DeleteFolder(string path, string name)  //удаление каталога
        {
            MessageBoxResult result = MessageBox.Show($"Удалить каталог: {name} ?", Title, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Directory.Delete(path, true);
                    MessageBox.Show($"Каталог {path} удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public void DeleteItem(string path, string name)    //удаление файла
        {
            MessageBoxResult result = MessageBox.Show($"Удалить файл: {name} ?", Title, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    File.Delete(path);
                    MessageBox.Show($"Файл {path} удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public string ReadTxt(string path) //читаем только тхт
        {
            string result = "";
            using (StreamReader reader = new StreamReader(path))
            {
               return result = reader.ReadToEnd();
            }
        }
        public void Data(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string extension = fileInfo.Extension.ToString();
            if (extension == ".png" || extension == ".bmp" || extension == ".jpg")
            {
                //выводим картинки
                Content = "";
                ImgVisibl = "Visible";
                TxtVisibl = "Hidden";
                HexVisibl = "Hidden";
                Content = path;
            }
            else if(extension == ".txt")
            {
                Content = "";
                ImgVisibl = "Hidden";
                TxtVisibl = "Visible";
                HexVisibl = "Hidden";
                var txt = ReadTxt(path);
                Content = txt.ToString();
            }
            else
            {
                Content = "";
                ImgVisibl = "Hidden";
                TxtVisibl = "Hidden";
                HexVisibl = "Visible";
                //читаем файл в нех
                var result = Hex16(GetHex(path));
                Content = result;
            }
        }
    }
}
