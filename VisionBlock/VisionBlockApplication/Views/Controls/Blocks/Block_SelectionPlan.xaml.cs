﻿using System;
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
    /// Logique d'interaction pour BlockSeuillage.xaml
    /// </summary>
    public partial class Block_SelectionPlan : UserControl
    {
        public Block_SelectionPlan()
        {
            InitializeComponent();
            this.DataContext = new ViewModelBlock_SelectionPlan();
        }

        public Block_SelectionPlan(ViewModelBlock_SelectionPlan block)
        {
            InitializeComponent();
            this.DataContext = block;
        }
    }
}
