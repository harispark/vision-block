using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VisionBlockApplication.Core;
using VisionBlockApplication.ViewModels.Controls.Blocks;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.ViewModels.Pages;

namespace VisionBlockApplication.ViewModels.Controls
{
    public class ViewModel_ImageSelector : ViewModel_Base
    {

        FileSystemWatcher watcher;
        public ViewModel_ImageSelector()
        {
            setWatcher(_folderName);
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            refreshList();
        }

        void refreshList()
        {
            Application app = System.Windows.Application.Current;
            if (app != null)
                app.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(refreshListOnUIThread), "");
        }

        object refreshListOnUIThread(object str)
        {
            string supportedExtensions = ".jpg,.gif,.png,.bmp,jpeg,.tif,.tiff";
            ImageCollection = new ObservableCollection<ViewModel_ImageFile>();
            foreach (string imageFile in Directory.GetFiles(FolderName, "*.*", SearchOption.TopDirectoryOnly).Where(s => !string.IsNullOrEmpty(Path.GetExtension(s)) && supportedExtensions.Contains(Path.GetExtension(s).ToLower())))
            {
                ImageCollection.Add(new ViewModel_ImageFile(imageFile));
            }
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                SelectedImageData = ImageCollection?.FirstOrDefault();
            return 1;
        }

        #region FolderName
        private string _folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        public string FolderName
        {
            get
            {
                return _folderName;
            }
            set
            {
                this._folderName = value;
                this.OnPropertyChanged(nameof(this.FolderName));
                this.OnPropertyChanged(nameof(this.DisplayedFolderName));
                setWatcher(_folderName);
            }
        }
        #endregion

        #region DisplayedFolderName
        public string DisplayedFolderName
        {
            get => FolderName.Substring(FolderName.LastIndexOf("\\")) ?? "";
        }
        #endregion

        #region ImageCollection
        private ObservableCollection<ViewModel_ImageFile> _imageCollection;
        public ObservableCollection<ViewModel_ImageFile> ImageCollection
        {
            get
            {
                return _imageCollection;
            }
            set
            {
                this._imageCollection = value;
                this.OnPropertyChanged(nameof(this.ImageCollection));
            }
        }
        #endregion

        #region selectedImageData
        private ViewModel_ImageFile _selectedImageData;
        public ViewModel_ImageFile SelectedImageData
        {
            get
            {
                return _selectedImageData;
            }
            set
            {
                _selectedImageData = value;
                this.OnPropertyChanged(nameof(this.SelectedImageData));
                var a = ((ViewModel_MainWindow)Application.Current.MainWindow.DataContext).EmptyBlocks.Where(x => x.DataContext.GetType() == typeof(ViewModelBlock_OuvertureImage));

                List<ViewModel_EmptyBlock> blocks = ((ViewModel_MainWindow)Application.Current.MainWindow.DataContext).EmptyBlocks.Select(x => (ViewModel_EmptyBlock)x.DataContext).ToList();
                foreach (ViewModel_EmptyBlock vm in blocks)
                {
                    vm.BlockImage = null;
                    if (vm.GetType() == typeof(ViewModelBlock_OuvertureImage))
                    {
                        if (SelectedImageData != null)
                        {
                            ((ViewModelBlock_OuvertureImage)vm).SetImage(SelectedImageData);
                            vm.ErrorIconVisibility = (vm.CanBeExecuted() == true) ? Visibility.Collapsed : Visibility.Visible;
                        }
                    } 
                }

                (Application.Current.MainWindow.DataContext as ViewModel_MainWindow).RemoveSelectedItem();
            }
        }
        #endregion

        void setWatcher(string _folderWatcherName)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = _folderWatcherName;

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName;

            // Only watch text files.
            //watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            refreshList();
        }

        #region ChangeDirectoryCommand
        protected ICommand _changeDirectoryCommand;
        public ICommand ChangeDirectoryCommand
        {
            get
            {
                if (_changeDirectoryCommand == null)
                {
                    _changeDirectoryCommand = new RelayCommand(p => { this.Execute_ChangeDirectoryCommand(); }, p => true);
                }
                return _changeDirectoryCommand;
            }
        }
        protected void Execute_ChangeDirectoryCommand()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                this.FolderName = dialog.SelectedPath;
            }
        }
        #endregion
    }
}
