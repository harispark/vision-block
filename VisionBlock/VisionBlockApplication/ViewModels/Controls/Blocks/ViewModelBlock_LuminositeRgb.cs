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
    public class ViewModelBlock_LuminositeRgb : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void LuminositeRgb(IntPtr data, int stride, int nbLig, int nbCol, byte brightness);

        public static Bitmap LuminositeRgb(Bitmap bmp, byte luminosite)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                LuminositeRgb(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, luminosite);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        public ViewModelBlock_LuminositeRgb()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Color;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Color;
        }

        public const string NAME = "Luminosité";

        public override string DescriptionButton { get { return "Luminosité en RGB"; } }

        public override string NameButton { get { return NAME; } }

        public override Type xamlBlockType { get { return typeof(Block_LuminositeRgb); } }


        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = LuminositeRgb(CloneBitmapThreadSafe(imgs.First()), LuminositeValue);
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

        #region LuminositeValue
        private byte _luminositeValue = 0;

        public byte LuminositeValue
        {
            get
            {
                return _luminositeValue;
            }
            set
            {
                if(value>=0 && value <= 255)
                {
                    _luminositeValue = value;
                }
                this.OnPropertyChanged(nameof(LuminositeValue));
            }
        }
        #endregion
    }
}
