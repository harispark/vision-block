using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace VisionBlockApplication.Views.Controls
{
    public partial class BasicColumn : UserControl
    {
        public BasicColumn()
        {
            InitializeComponent();

            Random rdn = new Random();
            var hee = new ChartValues<int>();
            for (int i = 0; i < 255; i++)
            {
                hee.Add(rdn.Next(0, 100000));
            }

            Labels = new string[255];

            for (int i = 0; i < 255; i++)
            {
                Labels[i] = i.ToString();
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = hee
                }
            };

            ////adding series will update and animate the chart automatically
            //SeriesCollection.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            //Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}
