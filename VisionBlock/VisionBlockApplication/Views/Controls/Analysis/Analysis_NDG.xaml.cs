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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Controls;
using VisionBlockApplication.ViewModels.Controls.Analysis;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.Views.Controls.Analysis
{
    /// <summary>
    /// Logique d'interaction pour Analysis_NDG.xaml
    /// </summary>
    public partial class Analysis_NDG : UserControl
    {
        public Analysis_NDG()
        {
            InitializeComponent();
            this.DataContext = new ViewModel_Analysis_NDG();
        }
    }
}




        