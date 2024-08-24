using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class ChangeTypeConverter : SingletonValueConverterBase<ChangeTypeConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
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

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
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
