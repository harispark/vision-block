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
using VisionBlockApplication.ViewModels.Controls.Blocks;

namespace VisionBlockApplication.Views.Controls.Blocks
{
    /// <summary>
    /// Logique d'interaction pour Block_RgbFromPlans.xaml
    /// </summary>
    public partial class Block_RgbFromPlans : UserControl
    {
        public Block_RgbFromPlans()
        {
            InitializeComponent();
            this.DataContext = new ViewModelBlock_RgbFromPlans();
        }

        public Block_RgbFromPlans(ViewModelBlock_RgbFromPlans block)
        {
            InitializeComponent();
            this.DataContext = block;
        }
    }
}
