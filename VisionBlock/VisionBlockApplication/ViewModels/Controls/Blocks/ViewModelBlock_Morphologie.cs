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
    public class ViewModelBLock_Morphologie : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void Morphologie(IntPtr data, int stride, int nbLig, int nbCol, int indexMorpho, int v4v8);

        public static Bitmap Morphologie(Bitmap bmp, int indexMorpho, int v4v8)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                Morphologie(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, indexMorpho, v4v8);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        static int v4v8 = 0;
        public ViewModelBLock_Morphologie()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Binaire;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.Binaire;

            listeMorphos = new List<String>(){"érosion","dilatation"};

            this.SelectedMorpho = listeMorphos[0];
        }

        public const string NAME = "Morphologie";
        public override string DescriptionButton { get { return "Opération de morphologie V4 ou V8"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_Morphologie); } }

        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            int a = this.ListeMorphos.FindIndex(x => x.StartsWith(SelectedMorpho));
            ImageToProcess = Morphologie(CloneBitmapThreadSafe(imgs.First()), a, 0);
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

        [JsonIgnore]
        private List<String> listeMorphos;
        [JsonIgnore]
        public List<String> ListeMorphos { get => listeMorphos; set => this.listeMorphos = value; }

        [JsonIgnore]
        private String selectedMorpho;
        [JsonIgnore]
        public String SelectedMorpho { get => selectedMorpho; set => selectedMorpho = value; }

        public ICommand ToggleBaseCommand { get; } = new RelayCommand(o => ApplyBase((bool)o));

        private static void ApplyBase(bool v4v8Toggle)
        {
            if (!v4v8Toggle)
                v4v8 = 0;
            else
                v4v8 = 1;
        }
    }
}
