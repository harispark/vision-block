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
    public partial class Block_Inversion : UserControl
    {
        public Block_Inversion()
        {
            InitializeComponent();
            this.DataContext = new ViewModelBlock_Inversion();
        }

        public Block_Inversion(ViewModelBlock_Inversion block)
        {
            InitializeComponent();
            this.DataContext = block;
        }
    }
}
