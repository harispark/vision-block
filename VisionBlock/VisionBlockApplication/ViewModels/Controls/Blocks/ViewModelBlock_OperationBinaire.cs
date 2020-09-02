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
    public class ViewModelBlock_OperationBinaire : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern unsafe void OperateurBinaire(IntPtr data, int stride, int nbLig, int nbCol, IntPtr data2, int stride2, int nbLig2, int nbCol2, string operateur);

        public struct MOMENTS
        {
            int min;
            int max;
            int mediane;
            double moyenne;
            double ecartType;
        };

        public static Bitmap OperateurBinaire(Bitmap bmp1, Bitmap bmp2, string operateur)
        {
            unsafe
            {
                Rectangle BoundsRect1 = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
                BitmapData bmpData1 = bmp1.LockBits(BoundsRect1, ImageLockMode.WriteOnly, bmp1.PixelFormat);
                Rectangle BoundsRect2 = new Rectangle(0, 0, bmp2.Width, bmp2.Height);
                BitmapData bmpData2 = bmp2.LockBits(BoundsRect2, ImageLockMode.WriteOnly, bmp2.PixelFormat);

                OperateurBinaire(bmpData1.Scan0, bmpData1.Stride, bmp1.Height, bmp1.Width, bmpData2.Scan0, bmpData2.Stride, bmp2.Height, bmp2.Width, operateur);

                bmp1.UnlockBits(bmpData1);
                bmp2.UnlockBits(bmpData2);
            }
            return bmp1;
        }

        public ViewModelBlock_OperationBinaire()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Binaire;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Binaire;
        }

        public const string NAME = "Binaire";
        public override string DescriptionButton { get { return "Opération binaire ET OU"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_OperationBinaire); } }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = OperateurBinaire(CloneBitmapThreadSafe(imgs[0]), CloneBitmapThreadSafe(imgs[1]), _choiceOperator ? "ou" : "et");
        }

        public override int NumberOfInputBlock { get { return 2; } }
        public override bool CanBeExecuted()
        {
            if (parents != null && parents.Count() == NumberOfInputBlock && parents[0].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput && parents[1].ProcessingCategoryEnumOutput == ProcessingCategoryEnumInput)
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

        private bool _choiceOperator = false;

        public bool ChoiceOperator
        {
            get { return _choiceOperator; }

            set { _choiceOperator = value; this.OnPropertyChanged(nameof(ChoiceOperator)); }
        }
    }
}
