using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static ComputedConverters.Interop;

namespace ComputedConverters;

internal static class ImageExtension
{
    public static ImageSource? ToBitmapSource(this object? value, int? decodePixelWidth = null)
    {
        if (value == null)
        {
            return null;
        }

        if (value.GetType().FullName == "System.Drawing.Bitmap")
        {
            dynamic bitmap = value;
            nint hBitmap = bitmap.GetHbitmap();
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            _ = Gdi32.DeleteObject(hBitmap);
            return imageSource;
        }
        else if (value.GetType().FullName == "System.Drawing.Image")
        {
            dynamic image = value;
            using MemoryStream memoryStream = new();
            dynamic imageFormatPng = value.GetType().Assembly
                .GetType("System.Drawing.Imaging.ImageFormat")!
                .GetField("Png", BindingFlags.Public | BindingFlags.Static)!
                .GetValue(null)!;

            image.Save(memoryStream, imageFormatPng);

            BitmapImage imageSource = new();

            imageSource.BeginInit();
            imageSource.StreamSource = memoryStream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            if (decodePixelWidth != null)
            {
                imageSource.DecodePixelWidth = decodePixelWidth.Value;
            }
            imageSource.EndInit();
            imageSource.Freeze();

            return imageSource;
        }
        else if (value.GetType().FullName == "System.Drawing.Icon")
        {
            dynamic icon = value;
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        return value switch
        {
            Uri uri => uri.ToImageSource(decodePixelWidth),
            string uriString => new Uri(uriString).ToImageSource(decodePixelWidth),
            ImageSource => (ImageSource)value,
            _ => null,
        };
    }

    public static ImageSource ToImageSource(this Uri imageUri, int? decodePixelWidth = null)
    {
        BitmapImage imageSource = new();

        imageSource.BeginInit();
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.UriSource = imageUri;
        if (decodePixelWidth != null)
        {
            imageSource.DecodePixelWidth = decodePixelWidth.Value;
        }
        imageSource.EndInit();
        imageSource.Freeze();
        return imageSource;
    }

    public static object? ToBitmap(this BitmapSource source)
    {
#if NET5_0_OR_GREATER
        // Not supported.
        return null;
#else
        var bitmap = new System.Drawing.Bitmap(
            source.PixelWidth,
            source.PixelHeight,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

        var bitmapData = bitmap.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

        source.CopyPixels(
            Int32Rect.Empty,
            bitmapData.Scan0,
            bitmapData.Height * bitmapData.Stride,
            bitmapData.Stride);

        bitmap.UnlockBits(bitmapData);

        return bitmap;
#endif
    }
}
