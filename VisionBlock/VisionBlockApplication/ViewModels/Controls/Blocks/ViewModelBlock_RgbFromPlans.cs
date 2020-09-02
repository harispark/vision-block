using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionBlockApplication.Core;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.Views.Controls;
using VisionBlockApplication.Views.Controls.Blocks;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_RgbFromPlans : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void RgbFromPlans(IntPtr dataR, int strideR, int nbLigR, int nbColR, IntPtr dataG, int strideG, int nbLigG, int nbColG, IntPtr dataB, int strideB, int nbLigB, int nbColB);

        public static Bitmap RgbFromPlans(Bitmap bmpR, Bitmap bmpG, Bitmap bmpB)
        {
            unsafe
            {
                Rectangle BoundsRectR = new Rectangle(0, 0, bmpR.Width, bmpR.Height);
                BitmapData bmpDataR = bmpR.LockBits(BoundsRectR, ImageLockMode.WriteOnly, bmpR.PixelFormat);
                Rectangle BoundsRectG = new Rectangle(0, 0, bmpG.Width, bmpG.Height);
                BitmapData bmpDataG = bmpG.LockBits(BoundsRectG, ImageLockMode.WriteOnly, bmpG.PixelFormat);
                Rectangle BoundsRectB = new Rectangle(0, 0, bmpB.Width, bmpB.Height);
                BitmapData bmpDataB = bmpB.LockBits(BoundsRectB, ImageLockMode.WriteOnly, bmpB.PixelFormat);

                RgbFromPlans(bmpDataR.Scan0, bmpDataR.Stride, bmpR.Height, bmpR.Width, bmpDataG.Scan0, bmpDataG.Stride, bmpG.Height, bmpB.Width, bmpDataB.Scan0, bmpDataB.Stride, bmpB.Height, bmpB.Width);

                bmpR.UnlockBits(bmpDataR);
                bmpG.UnlockBits(bmpDataG);
                bmpB.UnlockBits(bmpDataB);
            }
            return bmpR;
        }

        public ViewModelBlock_RgbFromPlans()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.GreyScale;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Color;
        }

        public const string NAME = "Création RGB";
        public override string DescriptionButton { get { return "Création d'une image RGB depuis 3 images NDG"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_RgbFromPlans); } }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = RgbFromPlans(CloneBitmapThreadSafe(imgs[0]), CloneBitmapThreadSafe(imgs[1]), CloneBitmapThreadSafe(imgs[2]));
        }

        public override int NumberOfInputBlock { get { return 3; } }
        public override bool CanBeExecuted()
        {
            if (parents != null && parents.Count() == NumberOfInputBlock && 
                parents[0].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput && 
                parents[1].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput &&
                parents[2].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput)
            {
                return true;
            }
            this.MessageErreurExecutionBlock = "Il manque un ou plusieurs parents";
            return false;
        }

        public override bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            if (typeIn == ProcessingCategoryEnumInput && (parents == null || (parents.Count() < NumberOfInputBlock)))
            {
                return true;
            }
            return false;
        }
    }
}
