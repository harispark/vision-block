using VisionBlockApplication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using VisionBlockApplication.Views.Controls;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using VisionBlockApplication.Models;
using VisionBlockApplication.Divers;
using VisionBlockApplication.ViewModels.Controls;
using VisionBlockApplication.ViewModels.Misc;
using Microsoft.Win32;
using VisionBlockApplication.Views.Controls.Blocks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using VisionBlockApplication.ViewModels.Controls.Blocks;
using System.Drawing;
using System.Threading;
using MaterialDesignThemes.Wpf;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.ViewModels.Pages
{
    public class ViewModel_MainWindow : ViewModel_Base
    {
        private static ObservableCollection<ViewModel_NavigationButton> _unfilteredButtons;

        public ViewModel_MainWindow()
        {
            _unfilteredButtons = new ObservableCollection<ViewModel_NavigationButton>();
            var allProcessingBlocks = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.BaseType == typeof(ViewModel_EmptyBlock)).ToList();

            foreach(var processingBlock in allProcessingBlocks)
            {
                var instanciatedProcessingBlock = (ViewModel_EmptyBlock)Activator.CreateInstance(processingBlock);
                if (instanciatedProcessingBlock.IsEnable == true)
                {
                    var newNavigationButton = new ViewModel_NavigationButton { NameButton = instanciatedProcessingBlock.NameButton, ProcessingCategoryInput = (int)instanciatedProcessingBlock.ProcessingCategoryEnumInput, ProcessingCategoryOutput = (int)instanciatedProcessingBlock.ProcessingCategoryEnumOutput, Description = instanciatedProcessingBlock.DescriptionButton, BlockType = instanciatedProcessingBlock.xamlBlockType };
                    instanciatedProcessingBlock = null;
                    _unfilteredButtons.Add(newNavigationButton);
                }
            }
            
            runningProcesses = new List<Thread>();

            Execute_DeleteAllBlocksCommand(true);
        }

        public List<Thread> runningProcesses;

        #region Binding Buttons
        private ObservableCollection<ViewModel_NavigationButton> _buttons;
        public ObservableCollection<ViewModel_NavigationButton> Buttons
        {
            get
            {
                return this._buttons;
            }
            set
            {
                this._buttons = value;
                this.OnPropertyChanged(nameof(this.Buttons));
            }
        }
        #endregion

        #region Binding WindowTitle
        private string _windowTitle = "Vision Block";
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(this.WindowTitle));
            }
        }
        #endregion

        #region ButtonsFilterChangedCommand
        private ICommand _buttonsFilterChangedCommand;
        public ICommand ButtonsFilterChangedCommand
        {
            get
            {
                if (_buttonsFilterChangedCommand == null)
                {
                    _buttonsFilterChangedCommand = new RelayCommand(p => { this.Execute_ButtonsFilterChangedCommand(); }, p => CanExecute_ButtonsFilterChangedCommand);
                }
                return _buttonsFilterChangedCommand;
            }
        }

        private Boolean CanExecute_ButtonsFilterChangedCommand = true;
        private void Execute_ButtonsFilterChangedCommand()
        {
            Buttons = _unfilteredButtons;
            var ButtonsEnabled = new ObservableCollection<ViewModel_NavigationButton>(_unfilteredButtons.Where(x =>
               x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.Color) && ColorProcessingChecked
            || x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.GreyScale) && GreyScaleProcessingChecked
            || x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.Binaire) && BinaryProcessingChecked
            || x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.None) && NoneProcessingChecked
            || x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.All) && AllProcessingChecked
            || x.ProcessingCategoryInput == ((int)ProcessingCategoryEnum.Multiple) && MultipleProcessingChecked));
            Buttons = new ObservableCollection<ViewModel_NavigationButton>(Buttons.OrderBy(x => (x.ProcessingCategoryInput.ToString())));

            foreach (ViewModel_NavigationButton btn in Buttons)
                btn.Enable = false;

            foreach (ViewModel_NavigationButton btn in ButtonsEnabled)
                btn.Enable = true;
        }
        #endregion

        #region Binding ColorProcessingChecked

        private bool _colorProcessingChecked;
        public bool ColorProcessingChecked
        {
            get
            {
                return _colorProcessingChecked;
            }
            set
            {
                _colorProcessingChecked = value;
                this.OnPropertyChanged(nameof(this.ColorProcessingChecked));
            }
        }

        #endregion

        #region Binding GreyScaleProcessingChecked

        private bool _greyScaleProcessingChecked;
        public bool GreyScaleProcessingChecked
        {
            get
            {
                return _greyScaleProcessingChecked;
            }
            set
            {
                _greyScaleProcessingChecked = value;
                this.OnPropertyChanged(nameof(this.GreyScaleProcessingChecked));
            }
        }

        #endregion

        #region Binding BinaryProcessingChecked

        private bool _binaryProcessingChecked;
        public bool BinaryProcessingChecked
        {
            get
            {
                return _binaryProcessingChecked;
            }
            set
            {
                _binaryProcessingChecked = value;
                this.OnPropertyChanged(nameof(BinaryProcessingChecked));
            }
        }

        #endregion

        #region Binding NoneProcessingChecked

        private bool _noneProcessingChecked;
        public bool NoneProcessingChecked
        {
            get
            {
                return _noneProcessingChecked;
            }
            set
            {
                _noneProcessingChecked = value;
                this.OnPropertyChanged(nameof(NoneProcessingChecked));
            }
        }

        #endregion

        #region Binding DisplayAllProcessingChecked

        private bool _displayAllProcessingChecked;
        public bool DisplayAllProcessingChecked
        {
            get
            {
                return _displayAllProcessingChecked;
            }
            set
            {
                _displayAllProcessingChecked = value;

                if (_displayAllProcessingChecked)
                {
                    ColorProcessingChecked = GreyScaleProcessingChecked = BinaryProcessingChecked = NoneProcessingChecked = AllProcessingChecked = MultipleProcessingChecked = true;
                    Execute_ButtonsFilterChangedCommand();
                }
                else
                {
                    ColorProcessingChecked = GreyScaleProcessingChecked = BinaryProcessingChecked = NoneProcessingChecked = AllProcessingChecked = MultipleProcessingChecked = false;
                    Execute_ButtonsFilterChangedCommand();
                }
                this.OnPropertyChanged(nameof(DisplayAllProcessingChecked));
            }
        }

        #endregion

        #region Binding AllProcessingChecked

        private bool _allProcessingChecked;
        public bool AllProcessingChecked
        {
            get
            {
                return _allProcessingChecked;
            }
            set
            {
                _allProcessingChecked = value;
                this.OnPropertyChanged(nameof(AllProcessingChecked));
            }
        }

        #endregion

        #region Binding MultipleProcessingChecked

        private bool _multipleProcessingChecked;
        public bool MultipleProcessingChecked
        {
            get
            {
                return _multipleProcessingChecked;
            }
            set
            {
                _multipleProcessingChecked = value;
                this.OnPropertyChanged(nameof(MultipleProcessingChecked));
            }
        }

        #endregion

        #region DeleteAllBlocksCommand
        private ICommand _deleteAllBlocksCommand;
        public ICommand DeleteAllBlocksCommand
        {
            get
            {
                if (_deleteAllBlocksCommand == null)
                {
                    _deleteAllBlocksCommand = new RelayCommand(p => { this.Execute_DeleteAllBlocksCommand(true); }, p => CanExecute_DeleteAllBlocksCommand());
                }
                return _deleteAllBlocksCommand;
            }
        }

        private bool CanExecute_DeleteAllBlocksCommand()
        {
            return true;
        }

        private void Execute_DeleteAllBlocksCommand(bool initialiseWithBlockOpenImage)
        {
            if(this.EmptyBlocks != null)
                this.EmptyBlocks.Clear();
            if (this.Lines != null)
                this.Lines.Clear();

            (Application.Current.MainWindow as MainWindow).analysisControl.Content = null;
            this.EmptyBlocks = new ObservableCollection<UserControl>();
            this.Lines = new ObservableCollection<ViewModel_Line>();
            ViewModel_EmptyBlock.i = 0;
            
            if(initialiseWithBlockOpenImage)
            {
                Block_OuvertureImage block = new Block_OuvertureImage();
                ((ViewModel_EmptyBlock)block.DataContext).Position = new System.Windows.Point(50, 50);
                EmptyBlocks.Add(block);
            }

            ColorProcessingChecked = true;
            Execute_ButtonsFilterChangedCommand();
        }
        #endregion

        #region ExporterRapportHtml
        private ICommand _exportRapportCommand;
        public ICommand ExportRapportCommand
        {
            get
            {
                if (_exportRapportCommand == null)
                {
                    _exportRapportCommand = new RelayCommand(p => { this.Execute_ExportRapportCommand(); }, p => CanExecute_ExportRapportCommand());
                }
                return _exportRapportCommand;
            }
        }

        private bool CanExecute_ExportRapportCommand()
        {
            return true;
        }

        private void Execute_ExportRapportCommand()
        {
            //filtrage des blocs à display
            List<ViewModel_EmptyBlock> blocks = EmptyBlocks.Select(x => (ViewModel_EmptyBlock)x.DataContext)
                .Where(y => y.ProcessingRunningState == ProcessingRunningStateEnum.HasAlreadyRun
                && y.BlockImage != null).ToList();

            //RapportManager.RenderHtmlRapport("Rapport", blocks);
        }
        #endregion

        #region ExporterBlocs
        private ICommand _exportBlocsCommand;
        public ICommand ExportBlocsCommand
        {
            get
            {
                if (_exportBlocsCommand == null)
                {
                    _exportBlocsCommand = new RelayCommand(p => { this.Execute_ExportBlocsCommand(); }, p => CanExecute_ExportBlocsCommand());
                }
                return _exportBlocsCommand;
            }
        }

        private bool CanExecute_ExportBlocsCommand()
        {
            return true;
        }

        private void Execute_ExportBlocsCommand()
        {
            List<ViewModel_EmptyBlock> blocks = EmptyBlocks.Select(x => (ViewModel_EmptyBlock)x.DataContext).ToList();

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
            string serializedBlocks = JsonConvert.SerializeObject(blocks, settings);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json|*.json";
            saveFileDialog.DefaultExt = ".json";

            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, serializedBlocks, Encoding.UTF8);
        }
        #endregion

        #region ImporterBlocs
        private ICommand _importBlocsCommand;
        public ICommand ImportBlocsCommand
        {
            get
            {
                if (_importBlocsCommand == null)
                {
                    _importBlocsCommand = new RelayCommand(p => { this.Execute_ImportBlocsCommand(); }, p => CanExecute_ImportBlocsCommand());
                }
                return _importBlocsCommand;
            }
        }

        private bool CanExecute_ImportBlocsCommand()
        {
            return true;
        }

        private void Execute_ImportBlocsCommand()
        {
            Execute_DeleteAllBlocksCommand(false);
            List<ViewModel_EmptyBlock> deserializerBlocks;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json|*.json";
            openFileDialog.DefaultExt = ".json";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var allProcessingBlocks = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.BaseType == typeof(ViewModel_EmptyBlock)).ToList();
                    deserializerBlocks = JsonConvert.DeserializeObject<List<ViewModel_EmptyBlock>>(File.ReadAllText(openFileDialog.FileName), settings);
                    ObservableCollection<UserControl> addedEmptyBlocks = new ObservableCollection<UserControl>();

                    foreach (var block in deserializerBlocks)
                    {
                        Type typeBlock = block.GetType();
                        int i = allProcessingBlocks.FindIndex(x=>x == typeBlock);
                        if (i >= 0)
                        {
                            var typez = allProcessingBlocks[i];
                            object[] ooo = new object[]{ block };
                            addedEmptyBlocks.Add((UserControl)Activator.CreateInstance(block.xamlBlockType, ooo));
                        }
                    }

                    ObservableCollection<ViewModel_Line> addedLines = new ObservableCollection<ViewModel_Line>();
                    foreach (UserControl block in addedEmptyBlocks)
                    {
                        ViewModel_EmptyBlock vmBlock = (ViewModel_EmptyBlock)block.DataContext;
                        if (vmBlock.parents != null)
                        {
                            foreach (ViewModel_EmptyBlock vmBlockParent in vmBlock.parents)
                            {
                                ViewModel_Line vmLine = new ViewModel_Line();
                                vmLine.setViewModelFromAndTo(vmBlockParent, vmBlock);
                                addedLines.Add(vmLine);
                            }
                        }
                    }

                    if (EmptyBlocks == null)
                        EmptyBlocks = new ObservableCollection<UserControl>();
                    if (Lines == null)
                        Lines = new ObservableCollection<ViewModel_Line>();

                    foreach (UserControl block in addedEmptyBlocks)
                        this.EmptyBlocks.Add(block);

                    foreach (ViewModel_Line vmLine in addedLines)
                        this.Lines.Add(vmLine);
                }
                catch (Exception)
                {
                    MessageBox.Show("Format de fichier non reconnu");
                }

                foreach (var blockToCheck in EmptyBlocks)
                {
                    ((ViewModel_EmptyBlock)blockToCheck.DataContext).UpdateErrorMessage();
                }
            }

        }
        #endregion

        #region Empty Blocks
        private ObservableCollection<UserControl> _emptyBlocks;
        public ObservableCollection<UserControl> EmptyBlocks
        {
            get
            {
                return _emptyBlocks;
            }
            set
            {
                _emptyBlocks = value;
                this.OnPropertyChanged(nameof(this.EmptyBlocks));
            }
        }

        public void RemoveEmptyBlock(UserControl b)
        {
            RemoveSelectedItem();
            foreach(var blockToCheck in EmptyBlocks)
            {
                ((ViewModel_EmptyBlock)blockToCheck.DataContext).UpdateErrorMessage();
            }

            EmptyBlocks.Remove(b);
            List<ViewModel_Line> lineToDelete = new List<ViewModel_Line>();
            foreach (ViewModel_Line l in Lines)
            {
                if (Object.ReferenceEquals(l._viewModelFromRef, b.DataContext) || Object.ReferenceEquals(l._viewModelToRef, b.DataContext))
                    lineToDelete.Add(l);
            }

            foreach (ViewModel_Line l in lineToDelete)
            {
                Lines.Remove(l);
            }
        }


        #endregion

        #region TreeCommand
        private ICommand _treeCommand;
        public ICommand TreeCommand
        {
            get
            {
                if (_treeCommand == null)
                {
                    _treeCommand = new RelayCommand(p => { this.Execute_TreeCommand(); }, p => CanExecute_TreeCommand());
                }
                return _treeCommand;
            }
        }

        private bool CanExecute_TreeCommand()
        {
            //if (EmptyBlocks.Where(x => ((ViewModel_EmptyBlock)x.DataContext).LoadingGifVisibility == Visibility.Visible).Count() != 0)
            //    return false;
            return true;
        }

        private void Execute_TreeCommand()
        {
            RemoveSelectedItem();

            if (runningProcesses.Where(x => x.IsAlive).Count() > 0)
            {
                MessageBox.Show("Traitement en cours ...");
                return;
            }

            runningProcesses.Clear();
            if (EmptyBlocks.Count(x => ((ViewModel_EmptyBlock)x.DataContext).CanBeExecuted() == false) > 0)
            {
                MessageBox.Show("Au moins un des blocs n'est pas valide");
                return;
            }

            EmptyBlocks.ToList().ForEach(x =>
            {
                ((ViewModel_EmptyBlock)x.DataContext).ProcessingRunningState = ProcessingRunningStateEnum.MustRun;
                ((ViewModel_EmptyBlock)x.DataContext).LoadingGifVisibility = Visibility.Visible;
            }); // on set tous les blocks à l'état "doit run"

            foreach (UserControl block in EmptyBlocks)
            {
                ViewModel_EmptyBlock vmBlock = (ViewModel_EmptyBlock)block.DataContext;

                if (vmBlock.GetType() == typeof(ViewModelBlock_OuvertureImage))
                {
                    Thread parentProcessing = new Thread(() => vmBlock.Do());
                    parentProcessing.Start();
                    runningProcesses.Add(parentProcessing);
                }
            }
        }
        #endregion

        #region Binding Lines
        private ObservableCollection<ViewModel_Line> _lines;
        public ObservableCollection<ViewModel_Line> Lines
        {
            get
            {
                return this._lines;
            }
            set
            {
                this._lines = value;
                this.OnPropertyChanged(nameof(this.Lines));
            }
        }
        #endregion

        #region Translation
        private double _translateX;

        public double TranslateX
        {
            get
            {
                return _translateX;
            }
            set
            {
                _translateX = value;
                this.OnPropertyChanged(nameof(this.TranslateX));
            }
        }

        private double _translateY;

        public double TranslateY
        {
            get
            {
                return _translateY;
            }
            set
            {
                _translateY = value;
                this.OnPropertyChanged(nameof(this.TranslateY));
            }
        }
        #endregion

        #region ToggleD4RK
        public ICommand ToggleBaseCommand { get; } = new RelayCommand(o => ApplyBase((bool)o));

        private static void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
        }
        #endregion

        #region RemoveSelectedItem
        public void RemoveSelectedItem()
        {
            var SelectedBlock = this.EmptyBlocks.Where(x => (x.DataContext as ViewModel_EmptyBlock).IsSelected == true);

            if (SelectedBlock.Count() != 0)
                (((SelectedBlock.First()).DataContext) as ViewModel_EmptyBlock).IsSelected = false;

            this.DisplayAllProcessingChecked = true;
            if((Application.Current.MainWindow as MainWindow).analysisControl!=null)
                (Application.Current.MainWindow as MainWindow).analysisControl.Content = null;
        }
        #endregion

        #region FilterButtonsFromProcessingType
        public void FilterButtonsFromProcessingType(ProcessingCategoryEnum p)
        {
            this.DisplayAllProcessingChecked = false;
            switch (p)
            {
                case ProcessingCategoryEnum.All:
                    this.AllProcessingChecked = true;
                    break;
                case ProcessingCategoryEnum.Binaire:
                    this.BinaryProcessingChecked = true;
                    break;
                case ProcessingCategoryEnum.Color:
                    this.ColorProcessingChecked = true;
                    break;
                case ProcessingCategoryEnum.GreyScale:
                    this.GreyScaleProcessingChecked = true;
                    break;
                case ProcessingCategoryEnum.Multiple:
                    this.MultipleProcessingChecked = true;
                    break;
                case ProcessingCategoryEnum.None:
                    this.NoneProcessingChecked = true;
                    break; 
            }
            Execute_ButtonsFilterChangedCommand();
        }
        #endregion

        #region ImageFullScreenDisplay
        [JsonIgnore]
        private Visibility _imageFullScreenDisplay = Visibility.Collapsed;
        [JsonIgnore]
        public Visibility ImageFullScreenDisplay
        {
            get
            {
                return _imageFullScreenDisplay;
            }
            set
            {
                this._imageFullScreenDisplay = value;
                this.OnPropertyChanged(nameof(this.ImageFullScreenDisplay));
            }
        }
        #endregion

        #region ImageFullScreenSize
        private double _imageFullScreenSize;
        public double ImageFullScreenSize
        {
            get
            {
                return _imageFullScreenSize;
            }
            set
            {
                this._imageFullScreenSize = value;
                this.OnPropertyChanged(nameof(this.ImageFullScreenSize));
            }
        }
        #endregion
    }
}
