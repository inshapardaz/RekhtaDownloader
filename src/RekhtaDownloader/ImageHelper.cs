using RekhtaDownloader.Models;
using SkiaSharp;

namespace RekhtaDownloader
{
    internal static class ImageHelper
    {
        public static SKImage RearrangeImage(SKImage source, PageData data)
        {
            var cellSize = 50;
            var borderWidth = 16;
            using (var surface = SKSurface.Create(new SKImageInfo
                   {
                       Width = data.PageWidth > 0 ? data.PageWidth : source.Width,
                       Height = data.PageHeight > 0 ? data.PageHeight : source.Height,
                       ColorType = SKImageInfo.PlatformColorType,
                       AlphaType = SKAlphaType.Premul
                   }))
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.FilterQuality = SKFilterQuality.High;

                foreach (var sub in data.Sub)
                {
                    int sourceX = (sub.X1 * cellSize) + (sub.X1 * borderWidth);
                    int sourceY = (sub.Y1 * cellSize) + (sub.Y1 * borderWidth);
                    int targetX = (sub.X2 * cellSize);
                    int targetY = (sub.Y2 * cellSize);

                    var sourceRectangle = new SKRect(sourceX, sourceY, cellSize + borderWidth, cellSize + borderWidth);
                    var targetRectangle = new SKRect(targetX, targetY, cellSize + borderWidth, cellSize + borderWidth);
                    surface.Canvas.DrawImage(source, targetRectangle, sourceRectangle, paint);
                }

                surface.Canvas.Flush();
                return surface.Snapshot();
            }
        }

        public static SKImage ResizeImage(int scaleFactor, SKImage image)
        {
            int width = (int)(image.Width * ((double)scaleFactor / 10));
            int height = (int)(image.Height * ((double)scaleFactor / 10));
            using (var surface = SKSurface.Create(new SKImageInfo
                   {
                       Width = width,
                       Height = height,
                       ColorType = SKImageInfo.PlatformColorType,
                       AlphaType = SKAlphaType.Premul
                   }))
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.FilterQuality = SKFilterQuality.High;

                surface.Canvas.DrawImage(image, new SKRectI(0, 0, width, height), paint);
                surface.Canvas.Flush();

                using (var newImg = surface.Snapshot())
                {
                    return newImg;
                }
            }
        }

        public static byte[] ToByteArray(this SKImage img)
        {
            return img.Encode().ToArray();
        }
    }
}