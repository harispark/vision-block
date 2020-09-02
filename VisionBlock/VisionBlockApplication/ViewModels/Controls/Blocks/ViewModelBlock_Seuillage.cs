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
    public class ViewModelBlock_Seuillage : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void Seuillage(IntPtr data, int stride, int nbLig, int nbCol, int valSeuilMin, int valSeuilMax);

        public static Bitmap Seuillage(Bitmap bmp, int valSeuilMin, int valSeuilMax)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                Seuillage(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, valSeuilMin, valSeuilMax);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        public ViewModelBlock_Seuillage()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.GreyScale;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Binaire;
        }

        public const string NAME = "Seuil";
        public override string DescriptionButton { get { return "Sélection d'un plan de l'image"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Seuillage); } }


        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = Seuillage(CloneBitmapThreadSafe(imgs.First()), SeuilMin, SeuilMax);
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

        #region SeuilMin
        private int _seuilMin = 128;

        public int SeuilMin
        {
            get
            {
                return _seuilMin;
            }
            set
            {
                if(value>=0 && value <= 254)
                {
                    _seuilMin = value;
                    if (_seuilMin >= _seuilMax)
                        SeuilMax = _seuilMin + 1;
                }
                this.OnPropertyChanged(nameof(SeuilMin));
            }
        }
        #endregion

        #region SeuilMax
        private int _seuilMax = 255;

        public int SeuilMax
        {
            get
            {
                return _seuilMax;
            }
            set
            {
                if (value >= 1 && value <= 255)
                {
                    _seuilMax = value;
                    if (_seuilMin >= _seuilMax)
                        SeuilMin = SeuilMax - 1;
                }
                this.OnPropertyChanged(nameof(SeuilMax));
            }
        }
        #endregion
    }
}
