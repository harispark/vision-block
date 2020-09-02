using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.Views.Controls.Blocks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_Transparent : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void AjoutCanalAlpha(IntPtr dataParam, int strideParam, int nbLigParam, int nbColParam, IntPtr dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, byte r, byte g,byte b,byte precision);

        public ViewModelBlock_Transparent()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Color;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.RGBA;
        }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            this.ImageToProcess = AjoutCanalAlpha(CloneBitmapThreadSafe(imgs.First()),R,G,B,Tolerance);
        }

        public const string NAME = "Transparent";
        public override string DescriptionButton { get { return "Création d'un fond transparent"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Transparent); } }

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
            if (typeIn == ProcessingCategoryEnumInput && (parents == null || parents.Count() == 0))
            {
                return true;
            }
            return false;
        }

        public static Bitmap AjoutCanalAlpha(Bitmap imageBlock,byte r,byte g, byte b, byte precision)
        {
            Bitmap bitmapRetourARGB = new Bitmap(imageBlock.Width, imageBlock.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); ;
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, imageBlock.Width, imageBlock.Height);
                BitmapData bmpData = imageBlock.LockBits(BoundsRect, ImageLockMode.ReadOnly, imageBlock.PixelFormat);

                Rectangle BoundsRect2 = new Rectangle(0, 0, imageBlock.Width, imageBlock.Height);
                BitmapData bmpDataRetour8bits = bitmapRetourARGB.LockBits(BoundsRect2, ImageLockMode.WriteOnly, bitmapRetourARGB.PixelFormat);

                AjoutCanalAlpha(bmpData.Scan0, bmpData.Stride, imageBlock.Height, imageBlock.Width, bmpDataRetour8bits.Scan0, bmpDataRetour8bits.Stride, imageBlock.Height, imageBlock.Width, r,g,b,precision);

                imageBlock.UnlockBits(bmpData);
                bitmapRetourARGB.UnlockBits(bmpDataRetour8bits);
            }

            return bitmapRetourARGB;
        }

        #region Bindings R G B & Tolérance
        private byte _r = 255;
        public byte R
        {
            get
            {
                return _r;
            }
            set
            {
                this._r = value;
                this.OnPropertyChanged(nameof(R));
            }
        }

        private byte _g = 255;
        public byte G
        {
            get
            {
                return _g;
            }
            set
            {
                this._g = value;
                this.OnPropertyChanged(nameof(G));
            }
        }

        private byte _b = 255;
        public byte B
        {
            get
            {
                return _b;
            }
            set
            {
                this._b = value;
                this.OnPropertyChanged(nameof(B));
            }
        }

        private byte _tolerance =50;
        public byte Tolerance
        {
            get
            {
                return _tolerance;
            }
            set
            {
                this._tolerance = value;
                this.OnPropertyChanged(nameof(Tolerance));
            }
        }
        #endregion
    }
}
