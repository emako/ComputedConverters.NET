using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Enum), typeof(bool))]
public sealed class EnumToBoolConverter : SingletonValueConverterBase<EnumToBoolConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return false;
        }

        return value.Equals(parameter);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return DependencyProperty.UnsetValue;
        }

        return (bool)value ? parameter : DependencyProperty.UnsetValue;
    }
}
