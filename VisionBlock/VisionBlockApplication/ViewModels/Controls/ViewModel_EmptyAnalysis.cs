using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionBlockApplication.ViewModels.Controls
{
    abstract class ViewModel_EmptyAnalysis : ViewModel_Base
    {
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                this._seriesCollection = value;
                this.OnPropertyChanged(nameof(this.SeriesCollection));
            }
        }

        #region HistogrammeHeader
        private string _histogrammeHeader;

        public string HistogrammeHeader
        {
            get
            {
                return this._histogrammeHeader;
            }
            set
            {
                this._histogrammeHeader = value;
                this.OnPropertyChanged(nameof(this.HistogrammeHeader));
            }
        }
        #endregion

        public abstract void ExecuteCalculHisto();
    }
}
