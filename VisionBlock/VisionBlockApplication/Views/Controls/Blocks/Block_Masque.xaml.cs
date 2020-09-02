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
    public partial class Block_Masque : UserControl
    {
        public Block_Masque()
        {
            InitializeComponent();
            this.DataContext = new ViewModelBlock_Masque();
        }

        public Block_Masque(ViewModelBlock_Masque block)
        {
            InitializeComponent();
            this.DataContext = block;
        }
    }
}
