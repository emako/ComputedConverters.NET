using System;
using System.Globalization;

namespace ComputedConverters;

[ValueConversionDynamic("System.Drawing.Bitmap", "System.Drawing.Bitmap")]
[ValueConversionDynamic("System.Drawing.Image", "System.Drawing.Bitmap")]
public sealed class ImageToBitmapConverter : SingletonValueConverterBase<ImageToBitmapConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        Type valueType = value.GetType();

        if (valueType.FullName == "System.Drawing.Image")
        {
            // Use ctor: new Bitmap(image);
            return valueType.Assembly.GetType("System.Drawing.Bitmap")
                !.GetConstructor([valueType])
                ?.Invoke([value]);
        }
        else if (valueType.FullName == "System.Drawing.Bitmap")
        {
            return value;
        }
        return null;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
