using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionBlockApplication.Core;
using VisionBlockApplication.ViewModels.Controls.Blocks;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Controls;
using VisionBlockApplication.Views.Controls.Analysis;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.ViewModels.Controls
{
    public abstract class ViewModel_EmptyBlock : ViewModel_Base
    {
        public ViewModel_EmptyBlock()
        {
            i++;
            IsSelected = false;
        }

        // Override dans les classes dérivées
        public virtual string DescriptionButton { get { return "DangerZone"; } }
        public virtual string NameButton { get { return "DangerZone"; } }
        public virtual Type xamlBlockType { get { return null; } }
        public virtual int NumberOfInputBlock { get; }
        public virtual bool IsEnable { get { return true; } }

        // Liste des blocs
        public List<ViewModel_EmptyBlock> parents;
        public List<ViewModel_EmptyBlock> enfants;

        // Définition du traitement, assignées dans les classes dérivées
        public ProcessingCategoryEnum ProcessingCategoryEnumInput;
        public ProcessingCategoryEnum ProcessingCategoryEnumOutput;
        public ProcessingRunningStateEnum ProcessingRunningState;

        // Gestion visuelle 
        public bool isMoving = false;
        public static int i = 0;
        public System.Windows.Point deltaPositionForDragDrop;
       
        // Traitement spécifique override dans les blocs
        public abstract void ExecuteTraitementBlock(List<Bitmap> imgs);

        public virtual bool CanBeExecuted()
        {
            return true;
        }

        public virtual bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            return true;
        }

        [JsonIgnore]
        private Bitmap _imageToProcess;
        [JsonIgnore]
        public Bitmap ImageToProcess
        {
            get
            {
                if (_imageToProcess == null)
                    return null;
                lock (_imageToProcess)
                {
                    return _imageToProcess;
                }
            }
            set
            {
                _imageToProcess = value;
            }
        }

        #region Binding Position
        protected System.Windows.Point _position;
        public System.Windows.Point Position
        {
            get { return _position; }
            set
            {
                this._position = value;
                this.OnPropertyChanged(nameof(this.Position));
            }
        }
        #endregion

        #region FromPointPosition
        protected System.Windows.Point _fromPointPosition;
        public System.Windows.Point FromPointPosition
        {
            get { return _fromPointPosition; }
            set
            {
                this._fromPointPosition = value;
                this.OnPropertyChanged(nameof(this.FromPointPosition));
            }
        }
        #endregion

        #region ToPointPosition
        protected System.Windows.Point _toPointPosition;
        public System.Windows.Point ToPointPosition
        {
            get { return _toPointPosition; }
            set
            {
                this._toPointPosition = value;
                this.OnPropertyChanged(nameof(this.ToPointPosition));
            }
        }
        #endregion

        #region Binding BlockHeader
        protected string _blockHeader = "block";
        public string BlockHeader
        {
            get => _blockHeader;
            set
            {
                _blockHeader = value;
                this.OnPropertyChanged(nameof(this.BlockHeader));
            }
        }
        #endregion

        #region CloseBlockCommand
        protected ICommand _closeBlockCommand;
        public ICommand CloseBlockCommand
        {
            get
            {
                if (_closeBlockCommand == null)
                {
                    _closeBlockCommand = new RelayCommand(p => { this.Execute_CloseBlockCommand(); }, p => CanExecute_CloseBlockCommand());
                }
                return _closeBlockCommand;
            }
        }

        protected bool CanExecute_CloseBlockCommand()
        {
            return true;
        }

        protected void Execute_CloseBlockCommand()
        {
            foreach (UserControl tb in VisualTree.FindVisualChildren<UserControl>(Application.Current.MainWindow))
            {
                ViewModel_EmptyBlock a = (tb.DataContext) as ViewModel_EmptyBlock;

                // on nettoie les parents et enfants qui référencaient le bloc qu'on veut supprimer
                if (a == this)
                {
                    if (parents != null)
                        foreach (var parent in parents)
                        {
                            if (parent.enfants != null)
                                for (int ii = 0; ii < parent.enfants.Count; ii++)
                                    parent.enfants.Remove(this);
                        }

                    if (enfants != null)
                        foreach (var enfant in enfants)
                        {
                            if (enfant.parents != null)
                                for (int ii = 0; ii < enfant.parents.Count; ii++)
                                    enfant.parents.Remove(this);
                        }
                    ViewModel_MainWindow vm = Application.Current.MainWindow.DataContext as ViewModel_MainWindow;
                    vm.RemoveEmptyBlock(tb);
                }
            }
        }
        #endregion

        #region Binding ShadowOpacity
        private double _shadowOpacity;
        public double ShadowOpacity
        {
            get
            {
                return this._shadowOpacity;
            }
            set
            {
                this._shadowOpacity = value;
                this.OnPropertyChanged(nameof(this.ShadowOpacity));
            }
        }
        #endregion

        #region Binding ShadowColor
        private System.Windows.Media.Color _shadowColor;
        public System.Windows.Media.Color ShadowColor
        {
            get
            {
                return _shadowColor;
            }
            set
            {
                this._shadowColor = value;
                this.OnPropertyChanged(nameof(this.ShadowColor));
            }
        }
        #endregion

        #region Binding BlockImage
        [JsonIgnore]
        private WriteableBitmap _blockImage;
        [JsonIgnore]
        public WriteableBitmap BlockImage
        {
            get
            {
                if (_blockImage == null)
                    return null;
                lock (_blockImage)
                {
                    return _blockImage;
                }
            }
            set
            {
                _blockImage = value;
                this.OnPropertyChanged(nameof(this.BlockImage));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region Binding textParents

        public string textParents
        {
            get
            {
                string a = "Parents :";
                if (parents != null)
                    foreach (ViewModel_EmptyBlock vm in parents)
                    {
                        a += vm.BlockHeader + " ";
                    }
                return a;
            }
            set
            {
                this.OnPropertyChanged(nameof(this.textParents));
            }
        }
        #endregion

        #region Binding textEnfants

        public string textEnfants
        {
            get
            {
                string a = "Enfants : ";
                if (enfants != null)
                    foreach (ViewModel_EmptyBlock vm in enfants)
                    {
                        a += vm.BlockHeader + " ";
                    }
                return a;
            }
            set
            {
                this.OnPropertyChanged(nameof(this.textEnfants));
            }
        }

        #endregion

        public void Do()
        {
            ProcessingRunningState = ProcessingRunningStateEnum.IsRunning;

            if(parents != null)
            {
                var parentsToWait = parents.Where(x => x.ProcessingRunningState != ProcessingRunningStateEnum.HasAlreadyRun).Count();
                while(parentsToWait > 0)
                {
                    Thread.Sleep(50);
                    if(parents != null)
                    {
                        parentsToWait = parents.Where(x => x.ProcessingRunningState != ProcessingRunningStateEnum.HasAlreadyRun).Count();
                    }
                    else
                    {
                        parentsToWait = 0;
                    }
                }
            }

            // mmmh
            if (parents != null || this.GetType() == typeof(ViewModelBlock_OuvertureImage))
            {
                // On change l'état du bloc pour dire qu'il est déjà en cours d'execution
                ProcessingRunningState = ProcessingRunningStateEnum.IsRunning;
                
                // LOL
                if (this.GetType() != typeof(ViewModelBlock_OuvertureImage))
                    ExecuteTraitementBlock(parents.Select(x=>x.ImageToProcess).ToList());
                //else
                //    ExecuteTraitementBlock(null); si ça marche sans on peut supprimer

                 // réalisation du traitement de l'enfant
                this.LoadingGifVisibility = Visibility.Collapsed;

                // Maj de l'image du block
                RefreshBlockImageVisualisation();

                // On dit que le bloc a été exécuté(RIP)
                ProcessingRunningState = ProcessingRunningStateEnum.HasAlreadyRun;

                if (enfants != null)
                {
                    var enfantsToRun = enfants.Where(x => x.ProcessingRunningState == ProcessingRunningStateEnum.MustRun).ToList();
                    if (enfantsToRun.Count() > 0) // il y a des traitements parents à executer
                    {
                        List<Thread> threadsEnfants = new List<Thread>();
                        foreach (ViewModel_EmptyBlock vmBlock in enfantsToRun) // lancement des traitements parents
                        {
                            Thread parentProcessing = new Thread(() => vmBlock.Do());
                            threadsEnfants.Add(parentProcessing);
                            parentProcessing.Start();
                        }
                    }
                }
            }
        }

        // Couroutine pour maj image du bloc
        public void RefreshBlockImageVisualisation()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ImageToProcess == null)
                    return;
                var bmpData = ImageToProcess.LockBits(new Rectangle(0, 0, ImageToProcess.Width, ImageToProcess.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, ImageToProcess.PixelFormat);
                BitmapSource bmpSource;
                switch (ImageToProcess.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        bmpSource = BitmapSource.Create(bmpData.Width, bmpData.Height, ImageToProcess.HorizontalResolution, ImageToProcess.VerticalResolution, PixelFormats.Bgra32, null, bmpData.Scan0, bmpData.Stride * bmpData.Height, bmpData.Stride);
                        break;
                    default:
                        bmpSource = BitmapSource.Create(bmpData.Width, bmpData.Height, ImageToProcess.HorizontalResolution, ImageToProcess.VerticalResolution, PixelFormats.Bgr24, null, bmpData.Scan0, bmpData.Stride * bmpData.Height, bmpData.Stride);
                        break;
                }
                ImageToProcess.UnlockBits(bmpData);
                BlockImage = new WriteableBitmap(bmpSource);
            });
        }

        public static Bitmap CloneBitmapThreadSafe(Bitmap imgToClone)
        {
            lock(imgToClone)
            {
                return (Bitmap)imgToClone.Clone();
            }
        }

        public void UpdateErrorMessage()
        {
           var canBeExecuted = CanBeExecuted();
           ErrorIconVisibility = (canBeExecuted == true) ? Visibility.Collapsed : Visibility.Visible;
           MessageErreurExecutionBlock = (canBeExecuted == true) ? "" : MessageErreurExecutionBlock;
        }

        #region LoadingGifVisibility
        [JsonIgnore]
        private Visibility _loadingGifVisibility = Visibility.Collapsed;
        [JsonIgnore]
        public Visibility LoadingGifVisibility
        {
            get
            {
                return _loadingGifVisibility;
            }
            set
            {
                this._loadingGifVisibility = value;
                this.OnPropertyChanged(nameof(this.LoadingGifVisibility));
            }
        }
        #endregion

        #region LoadingGifAutoStart
        [JsonIgnore]
        private bool _loadingGifAutoStart = true;
        [JsonIgnore]
        public bool LoadingGifAutoStart
        {
            get
            {
                return _loadingGifAutoStart;
            }
            set
            {
                this._loadingGifAutoStart = value;
                this.OnPropertyChanged(nameof(this.LoadingGifAutoStart));
            }
        }
        #endregion

        #region ActionButtonsVisibility
        [JsonIgnore]
        private Visibility _actionButtonsVisibility = Visibility.Collapsed;

        [JsonIgnore]
        public Visibility ActionButtonsVisibility
        {
            get
            {
                return _actionButtonsVisibility;
            }
            set
            {
                this._actionButtonsVisibility = value;
                this.OnPropertyChanged(nameof(this.ActionButtonsVisibility));
            }
        }
        #endregion

        #region ErrorIconVisibility
        [JsonIgnore]
        private Visibility _errorIconVisibility = Visibility.Visible;

        [JsonIgnore]
        public Visibility ErrorIconVisibility
        {
            get
            {
                return _errorIconVisibility;
            }
            set
            {
                this._errorIconVisibility = value;
                this.OnPropertyChanged(nameof(this.ErrorIconVisibility));
            }
        }
        #endregion

        #region MessageErreurExecutionBlock
        [JsonIgnore]
        private string _messageErreurExecutionBlock;
        [JsonIgnore]
        public string MessageErreurExecutionBlock
        {
            get
            {
                return _messageErreurExecutionBlock;
            }
            set
            {
                _messageErreurExecutionBlock = value;
                this.OnPropertyChanged(nameof(this.MessageErreurExecutionBlock));
            }
        }
        #endregion

        #region IsSelected
        [JsonIgnore]
        private bool _isSelected = false;

        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if(value == true)
                {
                    (Application.Current.MainWindow.DataContext as ViewModel_MainWindow).RemoveSelectedItem();
                }
                _isSelected = value;
                if (_isSelected == true)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    (mainWindow.DataContext as ViewModel_MainWindow).FilterButtonsFromProcessingType(this.ProcessingCategoryEnumOutput);
                    switch (ProcessingCategoryEnumOutput)
                    {
                        case ProcessingCategoryEnum.Color:
                            mainWindow.analysisControl.Content = new Analysis_RGB();
                            break;
                        case ProcessingCategoryEnum.GreyScale:
                            mainWindow.analysisControl.Content = new Analysis_NDG();
                            break;
                        case ProcessingCategoryEnum.Binaire:
                            mainWindow.analysisControl.Content = new Analysis_BIN();
                            break;
                        default:
                            mainWindow.analysisControl.Content = null;
                            break;
                    }
                }

                this.OnPropertyChanged(nameof(this.IsSelected));
                this.OnPropertyChanged(nameof(this.HeaderFontWeight));
            }
        }
        #endregion

        #region
        [JsonIgnore]
        public FontWeight HeaderFontWeight
        {
            get
            {
                if(IsSelected==true)
                    return FontWeights.Bold;
                return FontWeights.Normal;
            }
        }
        #endregion

        ~ViewModel_EmptyBlock()
        {
            //Console.WriteLine("kill");
        }

        #region SaveImageCommand
        [JsonIgnore]
        protected ICommand _saveImageCommand;
        [JsonIgnore]
        public ICommand SaveImageCommand
        {
            get
            {
                if (_saveImageCommand == null)
                {
                    _saveImageCommand = new RelayCommand(p => { this.Execute_SaveImageCommand(); }, p => CanExecute_SaveImageCommand());
                }
                return _saveImageCommand;
            }
        }
        protected bool CanExecute_SaveImageCommand()
        {
            return this.BlockImage != null;
        }
        protected void Execute_SaveImageCommand()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = this.BlockHeader + ".png";
            saveFile.InitialDirectory = ((Application.Current.MainWindow as MainWindow).Selector.DataContext as ViewModel_ImageSelector).FolderName;

            if (saveFile.ShowDialog() == true)
            {
                WriteableBitmap clone = this.BlockImage.Clone();
                using (FileStream fileStream = new FileStream(saveFile.FileName, FileMode.OpenOrCreate))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(clone));
                    encoder.Save(fileStream);
                }
            }
        }
        #endregion
    }
}
