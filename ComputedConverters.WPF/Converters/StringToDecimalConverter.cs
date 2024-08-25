using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(decimal))]
public sealed class StringToDecimalConverter : SingletonValueConverterBase<StringToDecimalConverter>
{
    private static readonly NumberStyles DefaultNumberStyles = NumberStyles.Any;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var dec = value as decimal?;
        if (dec != null)
        {
            return dec.Value.ToString("G", culture ?? CultureInfo.InvariantCulture);
        }

        if (value is string str)
        {
            if (decimal.TryParse(str, DefaultNumberStyles, culture ?? CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return result;
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        return Convert(value, targetTypes, parameter, culture);
    }
}
