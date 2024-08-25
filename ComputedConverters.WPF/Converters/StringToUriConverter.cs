using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(object))]
public sealed class StringToUriConverter : SingletonValueConverterBase<StringToUriConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string uriString)
        {
            return new Uri(uriString);
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is Uri uri)
        {
            return uri.ToString();
        }

        return DependencyProperty.UnsetValue;
    }
}
