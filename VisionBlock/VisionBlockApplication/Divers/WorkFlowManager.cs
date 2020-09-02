using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionBlockApplication.ViewModels.Controls;

namespace VisionBlockApplication.Divers
{
    public class WorkFlowManager
    {
        //public static WriteableBitmap ExecuteAllFuckingBlocks(ObservableCollection<ViewModel_EmptyBlock> blocks, WriteableBitmap bmp)
        //{
        //    // on choppe l'image de la mainWindow
        //    var lastProcessedImage = bmp;

        //    foreach (var x in blocks)
        //    {
        //        // copie
        //        var copiedImage = lastProcessedImage.Clone();

        //        // on la modifie
        //        lastProcessedImage = x.traitement.DoImageProcessing(copiedImage);

        //        // on l'affiche
        //        x.BlockImage = lastProcessedImage; // l'image devrait pas plutôt être stocké sur le modayle ?
        //    }
        //    return lastProcessedImage;
        //}
    }
}
