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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionBlockApplication.ImageProcessingWrapper;
using VisionBlockApplication.ViewModels.Misc;
using VisionBlockApplication.Views.Controls;
using VisionBlockApplication.Views.Controls.Blocks;

namespace VisionBlockApplication.ViewModels.Controls.Blocks
{
    public class ViewModelBlock_SelectionPlan : ViewModel_EmptyBlock
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void GetPlan(IntPtr data, int stride, int nbLig, int nbCol, int nbPlan);

        public static Bitmap Plan(Bitmap bmp, int plan)
        {
            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                GetPlan(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, plan);
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        public ViewModelBlock_SelectionPlan()
        {
            this.BlockHeader = NAME;
            this.ProcessingCategoryEnumInput = Misc.ProcessingCategoryEnum.Color;
            this.ProcessingCategoryEnumOutput = Misc.ProcessingCategoryEnum.GreyScale;


            LinearGradientBrush moyenBrush = new LinearGradientBrush();
            moyenBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.0));
            moyenBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            moyenBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));

            LinearGradientBrush teinteBrush = new LinearGradientBrush();
            teinteBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            teinteBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.25));
            teinteBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
            teinteBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.75));
            teinteBrush.GradientStops.Add(new GradientStop(Colors.Cyan, 1.0));

            LinearGradientBrush saturationBrush = new LinearGradientBrush();
            saturationBrush.GradientStops.Add(new GradientStop(Colors.DarkBlue, 0.0));
            saturationBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.25));
            saturationBrush.GradientStops.Add(new GradientStop(Colors.White, 1.0));

            LinearGradientBrush luminositeBrush = new LinearGradientBrush();
            luminositeBrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            luminositeBrush.GradientStops.Add(new GradientStop(Colors.White, 1));

            this.ListePlan = new List<Plan>() { new Plan() { Name = "plan moyen"     , valeurPlan=3, Brush = moyenBrush },
                                                new Plan() { Name = "plan rouge"     , valeurPlan=0, Brush = new SolidColorBrush(Colors.Red) },
                                                new Plan() { Name = "plan vert"      , valeurPlan=1, Brush = new SolidColorBrush(Colors.Green) },
                                                new Plan() { Name = "plan bleu"      , valeurPlan=2, Brush = new SolidColorBrush(Colors.Blue) },
                                                new Plan() { Name = "plan teinte"    , valeurPlan=4, Brush = teinteBrush },//mettre à jour cpp
                                                new Plan() { Name = "plan saturation", valeurPlan=5, Brush = saturationBrush },//mettre à jour cpp
                                                new Plan() { Name = "plan luminosité", valeurPlan=6, Brush = luminositeBrush }};//mettre à jour cpp

            this.SelectedPlan = ListePlan[0];
        }

        public const string NAME = "Plan";
        public override string DescriptionButton { get { return "Sélection d'un plan de l'image"; } }
        public override string NameButton { get { return NAME; } }
        public override Type xamlBlockType { get { return typeof(Block_SelectionPlan); } }


        public override void ExecuteTraitementBlock(List<Bitmap> imgs)
        {
            ImageToProcess = Plan(CloneBitmapThreadSafe(imgs.First()), this.SelectedPlan.valeurPlan);
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
            if (typeIn == ProcessingCategoryEnumInput && (parents == null || parents.Count() == 0))
            {
                return true;
            }
            return false;
        }

        [JsonIgnore]
        private List<Plan> listePlan;
        [JsonIgnore]
        public List<Plan> ListePlan { get => listePlan; set => this.listePlan = value; }

        private Plan selectedPlan;
        public Plan SelectedPlan { get => selectedPlan; set => selectedPlan = value; }
    }

    public class Plan : ViewModel_Base
    { 
        private System.Windows.Media.Brush brush;
        public System.Windows.Media.Brush Brush { get => brush; set => brush = value; }

        private string name;
        public string Name { get => name; set => name = value; }

        public int valeurPlan;
    }
}
