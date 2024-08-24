using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class ChangeTypeConverter : IValueConverter
{
    public static ChangeTypeConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return System.Convert.ChangeType(value, targetType);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return System.Convert.ChangeType(value, targetType);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
