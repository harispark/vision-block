using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using VisionBlockApplication.ViewModels.Controls.Analysis;

namespace VisionBlockApplication.Views.Controls.Analysis
{
    /// <summary>
    /// Logique d'interaction pour Analysis_NDG.xaml
    /// </summary>
    public partial class Analysis_BIN : UserControl
    {
        public Analysis_BIN()
        {
            InitializeComponent();
            this.DataContext = new ViewModel_Analysis_BIN();
        }
    }
}
