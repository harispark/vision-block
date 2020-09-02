using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    class ViewModel_Analysis_RGB : ViewModel_EmptyAnalysis
    {
        public ViewModel_Analysis_RGB()
        {
            SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
            redBrush.Opacity = 0.5;

            SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
            greenBrush.Opacity = 0.5;

            SolidColorBrush blueBrush = new SolidColorBrush(Colors.Blue);
            blueBrush.Opacity = 0.5;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "R",
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(Colors.Red),
                    Fill = redBrush

                },
                new LineSeries
                {
                    Title = "G",
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(Colors.Green),
                    Fill = greenBrush
                },
                new LineSeries
                {
                    Title = "B",
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(Colors.Blue),
                    Fill = blueBrush
                }
            };

            ExecuteCalculHisto();
        }

        public override void ExecuteCalculHisto()
        {
            Bitmap bmp = (((Application.Current.MainWindow as MainWindow).DataContext as ViewModel_MainWindow).EmptyBlocks.Where(x => (x.DataContext as ViewModel_EmptyBlock).IsSelected == true).First().DataContext as ViewModel_EmptyBlock).ImageToProcess;
            if (bmp == null)
                return;

            int[][] histogramme = new int[3][];
            this.HistogrammeHeader = "Calcul histogramme en cours";
            var task = Task.Run(() =>
            {
                histogramme = ImageProcessingLibrary.HistogrammeFromRgb(ViewModel_EmptyBlock.CloneBitmapThreadSafe(bmp));
            });

            task.ContinueWith((t) =>
            {
                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                {
                    SeriesCollection[0].Values = new ChartValues<int>(histogramme[0]);
                    SeriesCollection[1].Values = new ChartValues<int>(histogramme[1]);
                    SeriesCollection[2].Values = new ChartValues<int>(histogramme[2]);
                    this.HistogrammeHeader = "Histogramme :";
                });
            });
        }

        
    }
}
