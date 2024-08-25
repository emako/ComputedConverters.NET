using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputedConverters;

[ValueConversion(typeof(byte[]), typeof(ImageSource))]
public sealed class ByteArrayToImageSourceConverter : SingletonValueConverterBase<ByteArrayToImageSourceConverter>
{
    /// <summary>
    /// Converts the incoming value from <see cref="byte"/>[] and returns the object of a type <see cref="ImageSource"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the binding target property. This is not implemented.</param>
    /// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
    /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
    /// <returns>An object of type <see cref="ImageSource"/>.</returns>
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int? decodePixelWidth = null;
        if (parameter is string pixelString)
        {
            decodePixelWidth = int.Parse(pixelString);
        }
        else if (parameter is int pixelInt)
        {
            decodePixelWidth = pixelInt;
        }
        else if (parameter is double pixelDouble)
        {
            decodePixelWidth = (int)pixelDouble;
        }

        if (value is byte[] imageBytes)
        {
            return imageBytes.ToImageSource(decodePixelWidth);
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

file static class ImageExtension
{
    public static ImageSource ToImageSource(this byte[] byteArray, int? decodePixelWidth = null)
    {
        BitmapImage imageSource = new();
        using MemoryStream memoryStream = new(byteArray);

        imageSource.BeginInit();
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.StreamSource = memoryStream;
        if (decodePixelWidth != null)
        {
            imageSource.DecodePixelWidth = decodePixelWidth.Value;
        }
        imageSource.EndInit();
        imageSource.Freeze();
        return imageSource;
    }
}
