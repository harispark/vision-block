//using RazorEngine;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Windows.Controls;
//using VisionBlockApplication.ViewModels.Controls;

//namespace VisionBlockApplication.Divers
//{
//    public static class RapportManager
//    {
//        public class RapportImage
//        {
//            public string NameImage { get; set; }

//            public string PathImage { get; set; }
//        }

//        public static void RenderHtmlRapport(string nomRapport, List<ViewModel_EmptyBlock> blocks)
//        {

//            string htmlFilePath = Environment.CurrentDirectory + "/Html/";
//            string templatePath = htmlFilePath + "template.cshtml";
//            string rapportPath = htmlFilePath + nomRapport + ".html";

//            List<RapportImage> dataRapport = GenerateImageData(blocks, htmlFilePath);

//            var compiledRapport = Razor.Parse(File.ReadAllText(templatePath), dataRapport);
//            File.WriteAllText(rapportPath, compiledRapport);
//            OpenReadRapport(rapportPath);
//        }

//        private static string GetImagesPath(ViewModel_EmptyBlock block, string pathForImage)
//        {
//            string name = ImageWPFManager.GenerateImageName() + ".bmp";
//            ImageWPFManager.SaveImage(name, pathForImage, block.BlockImage.Clone());
//            return name;
//        }

//        private static string GetImageName(ViewModel_EmptyBlock block)
//        {
//            //return block.traitement.Name;
//            return block.BlockHeader;
//        }

//        private static List<RapportImage> GenerateImageData(List<ViewModel_EmptyBlock> blocks, string pathForImages)
//        {
//            List<RapportImage> rapportImages = new List<RapportImage>();
//            RapportImage rapportImage;
//            foreach (ViewModel_EmptyBlock block in blocks)
//            {
//                rapportImage = new RapportImage();
//                rapportImage.PathImage = GetImagesPath(block, pathForImages);
//                rapportImage.NameImage = GetImageName(block);
//                rapportImages.Add(rapportImage);
//            }
//            return rapportImages;
//        }

//        private static void OpenReadRapport(string rapportPath)
//        {
//            System.Diagnostics.Process.Start(rapportPath);
//        }
//    }
//}
