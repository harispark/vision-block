using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VisionBlockApplication.Core;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.Views.Controls.Blocks;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_Lut: ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe void Lut(IntPtr data, int stride, int nbLig, int nbCol, byte[] lut);
        public override bool IsEnable { get { return false; } }

        public static Bitmap Lut(Bitmap bmp1, byte[] lut)
        {
            unsafe
            {
                Rectangle BoundsRect1 = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
                BitmapData bmpData1 = bmp1.LockBits(BoundsRect1, ImageLockMode.WriteOnly, bmp1.PixelFormat);

                Lut(bmpData1.Scan0, bmpData1.Stride, bmp1.Height, bmp1.Width, lut);
                bmp1.UnlockBits(bmpData1);
            }
            return bmp1;
        }

        public ViewModelBlock_Lut()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.GreyScale;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.GreyScale;
        }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            byte[] lut = new byte[256];
            Random rnd = new Random();
            rnd.NextBytes(lut);

            ImageToProcess = Lut(CloneBitmapThreadSafe(imgs[0]), lut);
        }

        public override int NumberOfInputBlock { get { return 1; } }
        public override bool CanBeExecuted()
        {
            if (parents != null && parents.Count() == NumberOfInputBlock && parents[0].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput)
            {
                return true;
            }
            this.MessageErreurExecutionBlock = "Il manque un parent";
            return false;
        }

        public override bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            if ((parents == null || parents.Count() == 0) && typeIn == ProcessingCategoryEnumInput)
            {
                return true;
            }
            return false;
        }

        public const string NAME = "Lut";
        public override string DescriptionButton { get { return "Application d'une lut"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Lut); } }
    }
}
