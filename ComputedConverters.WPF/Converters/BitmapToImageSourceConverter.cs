using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversionDynamic("System.Drawing.Bitmap", typeof(ImageSource))]
[ValueConversionDynamic("System.Drawing.Image", typeof(ImageSource))]
[ValueConversionDynamic("System.Drawing.Icon", typeof(ImageSource))]
[ValueConversion(typeof(string), typeof(ImageSource))]
[ValueConversion(typeof(Uri), typeof(ImageSource))]
[ValueConversion(typeof(ImageSource), typeof(ImageSource))]
public sealed class BitmapToImageSourceConverter : SingletonValueConverterBase<BitmapToImageSourceConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        return ImageExtension.ToBitmapSource(value);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
