using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(Color))]
public sealed class StringToColorConverter : SingletonValueConverterBase<StringToColorConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString)
        {
            return ColorConverter.ConvertFromString(inputString);
        }
        return value;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
        return string.Empty;
    }
}
