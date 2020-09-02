using System.Windows.Controls;
using VisionBlockApplication.ViewModels.Controls.Analysis;

namespace VisionBlockApplication.Views.Controls.Analysis
{
    /// <summary>
    /// Logique d'interaction pour Analysis_NDG.xaml
    /// </summary>
    public partial class Analysis_RGB : UserControl
    {
        public Analysis_RGB()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel_Analysis_RGB();
        }
    }
}