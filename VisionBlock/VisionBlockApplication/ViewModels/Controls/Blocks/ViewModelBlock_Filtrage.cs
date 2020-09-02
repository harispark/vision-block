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
    public class ViewModelBlock_Filtrage : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void Filtrage(IntPtr data, int stride, int nbLig, int nbCol, int surfaceMin, int surfaceMax, bool miseAZero);
        public override bool IsEnable { get { return false; } }

        public static Bitmap Filtrage(Bitmap bmp, int surfaceMin, int surfaceMax, bool miseAZero)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                Filtrage(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, surfaceMin, surfaceMax, miseAZero);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        public string SurfMin { get; set; }
        public string SurfMax { get; set; }
        
        public ViewModelBlock_Filtrage()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Binaire;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Binaire;
        }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = Filtrage(CloneBitmapThreadSafe(imgs.First()), 100, 200, true);
        }

        public override int NumberOfInputBlock { get { return 1; } }
        public override bool CanBeExecuted()
        {
            if (parents != null && parents.Count() == NumberOfInputBlock && parents.First().ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput)
            {
                return true;
            }
            this.MessageErreurExecutionBlock = "Ce bloc n'est pas relié à un parent";
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

        public const string NAME = "Filtrage";
        public override string DescriptionButton { get { return "Filtrage pour récupérations objets"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Filtrage); } }
    }
}
