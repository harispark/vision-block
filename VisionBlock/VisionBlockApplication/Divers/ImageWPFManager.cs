using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VisionBlockApplication.Divers
{
    public static class ImageWPFManager
    {
        public static void SaveImage(string filename, string path, BitmapSource imageToSave)
        {
            string filePath = path + filename;

            using (FileStream stream = new FileStream(filePath, FileMode.Create)) //peut-être mettre un vrai dossier ou save par la suite
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageToSave));
                encoder.Save(stream);
            }
        }

        //https://stackoverflow.com/questions/5868438/c-sharp-generate-a-random-md5-hash
        public static string GenerateImageName()
        {
            //MD5 md5 = MD5.Create();
            //byte[] inputBytes = Encoding.ASCII.GetBytes(DateTime.Now.Ticks.ToString());
            //byte[] hash = md5.ComputeHash(inputBytes);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < hash.Length; i++)
            //{
            //    sb.Append(hash[i].ToString("x2"));
            //}
            //return sb.ToString();
            return "sorry";
        }

        public static WriteableBitmap randomBitmap()
        {
            int width = 300;
            int height = 300;

            // Create a writeable bitmap (which is a valid WPF Image Source
            WriteableBitmap BlockImage = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            // Create an array of pixels to contain pixel color values
            uint[] pixels = new uint[width * height];

            int red;
            int green;
            int blue;
            int alpha;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int i = width * y + x;

                    red = 255;
                    green = 255 * y / height;
                    blue = 255 * (width - x) / width;
                    alpha = 128;

                    pixels[i] = (uint)((blue << 24) + (green << 16) + (red << 8) + alpha);
                }
            }

            // apply pixels to bitmap
            BlockImage.WritePixels(new Int32Rect(0, 0, 300, 300), pixels, width * 4, 0);

            return BlockImage;
        }
    }
}
