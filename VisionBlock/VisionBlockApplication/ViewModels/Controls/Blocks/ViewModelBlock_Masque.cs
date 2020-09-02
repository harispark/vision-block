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
    public class ViewModelBlock_Masque : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern unsafe void Masque(IntPtr data, int stride, int nbLig, int nbCol, IntPtr data2, int stride2, int nbLig2, int nbCol2);

        public static Bitmap Masque(Bitmap bmpNdg, Bitmap bmpBinaire)
        {
            unsafe
            {
                Rectangle BoundsRectNdg = new Rectangle(0, 0, bmpNdg.Width, bmpNdg.Height);
                BitmapData bmpNdgData = bmpNdg.LockBits(BoundsRectNdg, ImageLockMode.WriteOnly, bmpBinaire.PixelFormat);
                Rectangle BoundsRectBinaire = new Rectangle(0, 0, bmpBinaire.Width, bmpBinaire.Height);
                BitmapData bmpBinaireData = bmpBinaire.LockBits(BoundsRectBinaire, ImageLockMode.WriteOnly, bmpBinaire.PixelFormat);

                Masque(bmpNdgData.Scan0, bmpNdgData.Stride, bmpNdg.Height, bmpNdg.Width, bmpBinaireData.Scan0, bmpBinaireData.Stride, bmpBinaire.Height, bmpBinaire.Width);

                bmpNdg.UnlockBits(bmpNdgData);
                bmpBinaire.UnlockBits(bmpBinaireData);
            }
            return bmpNdg;
        }

        public ViewModelBlock_Masque()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Multiple;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.GreyScale;
        }

        public const string NAME = "Masque";
        public override string DescriptionButton { get { return "Masque binaire sur une image NDG"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Masque); } }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = Masque(CloneBitmapThreadSafe(imgs[0]), CloneBitmapThreadSafe(imgs[1]));
        }

        public override int NumberOfInputBlock { get { return 2; } }
        public override bool CanBeExecuted()
        {
            if (parents != null && parents.Count() == NumberOfInputBlock)
                    return true;
            else
                this.MessageErreurExecutionBlock = "Il manque un ou plusieurs parents";

            return false;
        }

        public override bool CanBeMapped(ProcessingCategoryEnum typeIn)
        {
            if (parents == null)
            {
                return true;
            }
            else
            {
                if (parents.First().ProcessingCategoryEnumOutput == ProcessingCategoryEnum.Binaire && typeIn == ProcessingCategoryEnum.GreyScale || parents.First().ProcessingCategoryEnumOutput == ProcessingCategoryEnum.GreyScale && typeIn == ProcessingCategoryEnum.Binaire)
                    return true;
            }
            return false;
        }
    }
}
