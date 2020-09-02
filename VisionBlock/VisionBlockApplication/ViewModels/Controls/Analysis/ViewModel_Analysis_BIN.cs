using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.ViewModels.Controls.Analysis
{
    class ViewModel_Analysis_BIN : ViewModel_EmptyAnalysis
    {
        static int _nombreElementsStatic = 0;

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int Etiquetage(IntPtr dataParam, int strideParam, int nbLigParam, int nbColParam, IntPtr dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, IntPtr nbRegions);
        Bitmap imageEtiquette;
        static IntPtr nbRegions;

        public ViewModel_Analysis_BIN()
        {
            nbRegions = Marshal.AllocHGlobal(1);
            SeriesCollection = new SeriesCollection();
            ExecuteCalculHisto();
        }

        public override void ExecuteCalculHisto()
        {
            Bitmap bmp = (((Application.Current.MainWindow as MainWindow).DataContext as ViewModel_MainWindow).EmptyBlocks.Where(x => (x.DataContext as ViewModel_EmptyBlock).IsSelected == true).First().DataContext as ViewModel_EmptyBlock).ImageToProcess;
            if (bmp == null)
                return;

            int[] histogramme = new int[255];
            this.HistogrammeHeader = "Calcul histogramme en cours";
            var task = Task.Run(() =>
            {
                histogramme = ImageProcessingLibrary.HistogrammeFromGreyscale(ViewModel_EmptyBlock.CloneBitmapThreadSafe(bmp));
                imageEtiquette = Etiquetage(ViewModel_EmptyBlock.CloneBitmapThreadSafe(bmp));
                NombreElements = _nombreElementsStatic;
            });

            

            task.ContinueWith((t) =>
            {
                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                {
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Values = new ChartValues<int> { histogramme[0], histogramme[255] }
                    });
                    this.HistogrammeHeader = "Histogramme :";


                    if (imageEtiquette == null)
                        return;
                    BitmapData bmpData = imageEtiquette.LockBits(new Rectangle(0, 0, imageEtiquette.Width, imageEtiquette.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, imageEtiquette.PixelFormat);

                    //Création d'une palette de 256 couleurs ni plus ni moins
                    List<System.Windows.Media.Color> Colors = new List<System.Windows.Media.Color>();
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                    for (int i = 0; i < 31; i++)
                    {
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 0, 255));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 255, 0));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 0, 255));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 255, 128));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 128, 128));
                        Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 255, 255));
                    }
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 0, 255));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 255, 0));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 0, 255, 255));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 0, 255));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 255, 0));
                    Colors.Add(System.Windows.Media.Color.FromArgb(255, 255, 255, 128));
                    BitmapPalette PLT = new BitmapPalette(Colors);

                    //Création de la BitmapSource avec la lut et les données de l'image étiquettée
                    BitmapSource secondBitmap = BitmapSource.Create(bmpData.Width, bmpData.Height, imageEtiquette.HorizontalResolution, imageEtiquette.VerticalResolution, PixelFormats.Indexed8, PLT, bmpData.Scan0, bmpData.Height * bmpData.Stride, bmpData.Stride);

                    imageEtiquette.UnlockBits(bmpData);
                    ImageEtiquette = new WriteableBitmap(secondBitmap);
                });
            });
        }

        public static Bitmap Etiquetage(Bitmap imageBlock)
        {
            Bitmap bitmapRetour8bits = new Bitmap(imageBlock.Width, imageBlock.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, imageBlock.Width, imageBlock.Height);
                BitmapData bmpData = imageBlock.LockBits(BoundsRect, ImageLockMode.ReadOnly, imageBlock.PixelFormat);

                Rectangle BoundsRect2 = new Rectangle(0, 0, imageBlock.Width, imageBlock.Height);
                BitmapData bmpDataRetour8bits = bitmapRetour8bits.LockBits(BoundsRect2, ImageLockMode.WriteOnly, bitmapRetour8bits.PixelFormat);

                _nombreElementsStatic = Etiquetage(bmpData.Scan0, bmpData.Stride, imageBlock.Height, imageBlock.Width, bmpDataRetour8bits.Scan0, bmpDataRetour8bits.Stride, imageBlock.Height, imageBlock.Width, nbRegions);

                imageBlock.UnlockBits(bmpData);
                bitmapRetour8bits.UnlockBits(bmpDataRetour8bits);
            }

            return bitmapRetour8bits;
        }

        #region Binding ImageEtiquette
        private WriteableBitmap _imageEtiquette;
        public WriteableBitmap ImageEtiquette
        {
            get
            {
                return _imageEtiquette;
            }
            set
            {
                this._imageEtiquette = value;

                this.OnPropertyChanged(nameof(this.ImageEtiquette));
            }
        }
        #endregion

        #region Binding NombreElements
        private int _nombreElements = 0;

        public int NombreElements
        {
            get
            {
                return _nombreElements;
            }
            set
            {
                this._nombreElements = value;
                this.OnPropertyChanged(nameof(NombreElements));
                this.OnPropertyChanged(nameof(DispNombreElements));
            }
        }

        #endregion

        public string DispNombreElements
        {
            get { return $"Nombre d'éléments étiquetés : {NombreElements.ToString()}"; }
        }
    }
}
