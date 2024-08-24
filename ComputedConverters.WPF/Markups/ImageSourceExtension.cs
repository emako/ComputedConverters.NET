using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static ComputedConverters.Interop;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(ImageSource))]
public sealed class ImageSourceExtension(object? value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public object? Value { get; set; } = value;

    public int? DecodePixelWidth { get; set; } = null;

    public ImageSourceExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Value != null)
        {
            if (Value.GetType().FullName == "System.Drawing.Bitmap")
            {
                dynamic bitmap = Value;
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
            else if (Value.GetType().FullName == "System.Drawing.Image")
            {
                dynamic image = Value;
                using MemoryStream memoryStream = new();
                dynamic imageFormatPng = Value.GetType().Assembly
                    .GetType("System.Drawing.Imaging.ImageFormat")!
                    .GetField("Png", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;

                image.Save(memoryStream, imageFormatPng);

                BitmapImage imageSource = new();

                imageSource.BeginInit();
                imageSource.StreamSource = memoryStream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                if (DecodePixelWidth != null)
                {
                    imageSource.DecodePixelWidth = DecodePixelWidth.Value;
                }
                imageSource.EndInit();
                imageSource.Freeze();

                return imageSource;
            }
        }

        return Value switch
        {
            Uri uri => uri.ToImageSource(DecodePixelWidth),
            string uriString => new Uri(uriString).ToImageSource(DecodePixelWidth),
            ImageSource => Value,
            _ => null,
        };
    }
}

file static class ImageExtension
{
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
}
