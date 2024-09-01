using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversion(typeof(Uri), typeof(ImageSource))]
[ValueConversion(typeof(string), typeof(ImageSource))]
public sealed class UriToImageSourceConverter : SingletonValueConverterBase<UriToImageSourceConverter>
{
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

        if (value is null)
        {
            return null;
        }
        else if (value is Uri uri)
        {
            return uri.ToImageSource(decodePixelWidth);
        }
        else if (value is string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out Uri? uriOk))
            {
                return uriOk.ToImageSource(decodePixelWidth);
            }
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
