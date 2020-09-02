using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.ViewModels.Controls.Analysis
{
    class ViewModel_Analysis_NDG : ViewModel_EmptyAnalysis
    {
        public ViewModel_Analysis_NDG()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
            brush.Opacity = 0.5;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "NDG",
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(Colors.Gray),
                    Fill = brush
                }
            };
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
            });

            task.ContinueWith((t) =>
            {
                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                {
                    ChartValues<int> chart;
                    chart = new ChartValues<int>();
                    chart.AddRange(histogramme);
                    SeriesCollection.First().Values = chart;
                    this.HistogrammeHeader = "Histogramme :";
                });
            });
        }
    }
}
