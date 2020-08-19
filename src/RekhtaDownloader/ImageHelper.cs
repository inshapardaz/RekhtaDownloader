using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using RekhtaDownloader.Models;

namespace RekhtaDownloader
{
    internal static class ImageHelper
    {
        public static Bitmap RearrangeImage(Bitmap source, PageData data)
        {
            var cellSize = 50;
            var borderWidth = 16;
            var target = new Bitmap(data.PageWidth > 0 ? data.PageWidth : source.Width,
                                    data.PageHeight > 0 ? data.PageHeight : source.Height);
            using (var gt = Graphics.FromImage(target))
            {
                foreach (var sub in data.Sub)
                {
                    int sourceX = (sub.X1 * cellSize) + (sub.X1 * borderWidth);
                    int sourceY = (sub.Y1 * cellSize) + (sub.Y1 * borderWidth);
                    int targetX = (sub.X2 * cellSize);
                    int targetY = (sub.Y2 * cellSize);

                    Rectangle sourceRectangle = new Rectangle(sourceX, sourceY, cellSize + borderWidth, cellSize + borderWidth);
                    Rectangle targetRectangle = new Rectangle(targetX, targetY, cellSize + borderWidth, cellSize + borderWidth);
                    gt.DrawImage(source, targetRectangle, sourceRectangle, GraphicsUnit.Pixel);
                }

                gt.Flush();
            }

            return target;
        }

        public static Bitmap ResizeImage(int scaleFactor, Bitmap image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int width = (int)(image.Width * ((double)scaleFactor / 10));
                int height = (int)(image.Height * ((double)scaleFactor / 10));
                Bitmap bitmap = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;

                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    graphics.DrawImage(image, rect);
                    bitmap.Save(memoryStream, ImageFormat.Jpeg);
                }
                return bitmap;
            }
        }
    }
}