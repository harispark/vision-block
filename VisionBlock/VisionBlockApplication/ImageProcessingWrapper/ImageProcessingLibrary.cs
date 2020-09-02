using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace VisionBlockApplication.ImageProcessingWrapper
{
    public static class ImageProcessingLibrary
    {

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void HistogrammeNdg(IntPtr data, int stride, int nbLig, int nbCol, out IntPtr hist);
        public static int[] HistogrammeFromGreyscale(Bitmap bmp)
        {
            IntPtr hist;
            int[] buffer = new int[256];

            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                HistogrammeNdg(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, out hist);
                Marshal.Copy(hist, buffer, 0, 256);
                bmp.UnlockBits(bmpData);
            }
            return buffer;
        }

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void HistogrammeRgb(IntPtr data, int stride, int nbLig, int nbCol, out IntPtr hist1, out IntPtr hist2, out IntPtr hist3);
        public static int[][] HistogrammeFromRgb(Bitmap bmp)
        {
            IntPtr hist1,hist2,hist3;
            int[][] buffer = new int[3][];
            buffer[0] = new int[256];
            buffer[1] = new int[256];
            buffer[2] = new int[256];

            unsafe
            {
                Rectangle BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                HistogrammeRgb(bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width, out hist1, out hist2, out hist3);
                Marshal.Copy(hist1, buffer[0], 0, 256);
                Marshal.Copy(hist2, buffer[1], 0, 256);
                Marshal.Copy(hist3, buffer[2], 0, 256);

                bmp.UnlockBits(bmpData);
            }
            return buffer;
        }


        public static Bitmap DrawHistogram(int maxVal, int width, int height, int[] histData)
        {
            Bitmap histo = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(histo);
            g.Clear(System.Drawing.SystemColors.Window);
            Pen penGray = new Pen(Brushes.DarkGray);

            for (var i = 0; i < histData.GetLength(0); i++)
            {
                var val =  (float)histData[i];
                val = (float)(val * (maxVal != 0 ? (float)height / (float)maxVal : 0.0));

                System.Drawing.Point s = new System.Drawing.Point(i, height);
                System.Drawing.Point e = new System.Drawing.Point(i, height - (int)val);
                g.DrawLine(penGray, s, e);
            }

            return histo;
        }

        public static Bitmap DrawHistogramRGBDegeu(int maxVal, int width, int height, int[,] histData)
        {
            Bitmap histo = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(histo);
            g.Clear(System.Drawing.SystemColors.Window);

            Pen penRed = new Pen(Brushes.Red);
            Pen penBlue = new Pen(Brushes.Blue);
            Pen penGreen = new Pen(Brushes.Green);

            for (var i = 0; i < histData.GetLength(0); i++)
            {
                var valRed = (float)histData[i,0];
                valRed = (float)(valRed * (maxVal != 0 ? (float)height / (float)maxVal : 0.0));
                var valBlue = (float)histData[i,1];
                valBlue = (float)(valBlue * (maxVal != 0 ? (float)height / (float)maxVal : 0.0));
                var valGreen = (float)histData[i,2];
                valGreen = (float)(valGreen * (maxVal != 0 ? (float)height / (float)maxVal : 0.0));

                System.Drawing.Point sR = new System.Drawing.Point(i, height);
                System.Drawing.Point eR = new System.Drawing.Point(i, height - (int)valRed);
                g.DrawLine(penRed, sR, eR);

                System.Drawing.Point sB = new System.Drawing.Point(i, height);
                System.Drawing.Point eB = new System.Drawing.Point(i, height - (int)valBlue);
                g.DrawLine(penBlue, sB, eB);

                System.Drawing.Point sG = new System.Drawing.Point(i, height);
                System.Drawing.Point eG = new System.Drawing.Point(i, height - (int)valGreen);
                g.DrawLine(penRed, sG, eG);
            }

            return histo;
        }
    }
}
