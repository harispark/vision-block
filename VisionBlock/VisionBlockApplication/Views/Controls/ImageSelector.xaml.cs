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
using VisionBlockApplication.ViewModels.Controls;

namespace VisionBlockApplication.Views.Controls
{
    /// <summary>
    /// Logique d'interaction pour ImageSelector.xaml
    /// </summary>
    public partial class ImageSelector : UserControl
    {
        public ImageSelector()
        {
            InitializeComponent();
            this.DataContext = new ViewModel_ImageSelector();
        }
    }
}
