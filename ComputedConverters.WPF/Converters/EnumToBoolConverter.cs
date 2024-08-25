using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
[ValueConversion(typeof(Enum), typeof(bool))]
public sealed class EnumToBoolConverter : SingletonValueConverterBase<EnumToBoolConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return false;
        }

        if (parameter is string parameterString)
        {
            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            var parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        return value.Equals(parameter);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return DependencyProperty.UnsetValue;
        }

        if (parameter is string parameterString)
        {
            return (bool)value ? Enum.Parse(targetType, parameterString) : DependencyProperty.UnsetValue;
        }

        return (bool)value ? parameter : DependencyProperty.UnsetValue;
    }
}
