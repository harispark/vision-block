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
using System.Windows.Media.Imaging;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.Views.Controls;
using VisionBlockApplication.Views.Controls.Blocks;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_ContrasteRgb : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void ContrasteRgb(IntPtr data, int stride, int nbLig, int nbCol, double contraste);

        public static Bitmap ContrasteRgb(Bitmap bmp, double contraste)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                ContrasteRgb(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, contraste);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        public ViewModelBlock_ContrasteRgb()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Color;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Color;
        }

        public const string NAME = "Contraste";
        public override string DescriptionButton { get { return "Augmentation des contrastes d'une image RGB"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_ContrasteRgb); } }


        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = ContrasteRgb(CloneBitmapThreadSafe(imgs.First()), ContrasteValue);
        }

        public override int NumberOfInputBlock { get { return 1; } }
        public override bool CanBeExecuted()
        {
            if(parents != null && parents.Count() == NumberOfInputBlock && parents.First().ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput)
            {
               return true;
            }
            this.MessageErreurExecutionBlock = "Ce bloc n'est pas relié à un parent";
            return false;
        }

        public override bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            if (typeIn == ProcessingCategoryEnumInput && (parents == null || parents.Count() == 0))
            {
                return true;
            }
            return false;
        }

        #region ContrasteValue
        private double _contrasteValue = 1;

        public double ContrasteValue
        {
            get
            {
                return _contrasteValue;
            }
            set
            {
                if(value>=0 && value <= 5)
                {
                    _contrasteValue = value;
                }
                this.OnPropertyChanged(nameof(ContrasteValue));
            }
        }
        #endregion
    }
}
