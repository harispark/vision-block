using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VisionBlockApplication.Core;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Controls.Blocks;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_OuvertureImage : ViewModel_EmptyBlock
    {
        public ViewModelBlock_OuvertureImage()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.None;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Color;
            var sel = (Application.Current.MainWindow as MainWindow).Selector;
            if(sel != null)
            {
                var imgFile = (sel.DataContext as ViewModel_ImageSelector).SelectedImageData;
                if(imgFile != null)
                    this.SetImage(imgFile);
            }
        }

        public const string NAME = "Ouverture";
        public override string DescriptionButton { get { return "Ouverture d'une image"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_OuvertureImage); } }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
        }

        public override int NumberOfInputBlock { get { return 0; } }
        public override bool CanBeExecuted()
        {
            if (this.BlockImage != null)
            {
                return true;
            }
            this.MessageErreurExecutionBlock = "Selectionner une image";
            return false;
        }

        public override bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            return false;
        }

        internal void SetImage(ViewModel_ImageFile imgFile)
        {
            BlockImage = imgFile.BitmapData;
            ImageToProcess = new System.Drawing.Bitmap(imgFile.Filename);
        }
    }
}
