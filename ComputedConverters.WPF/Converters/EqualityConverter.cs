using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public sealed class EqualityConverter : IValueConverter
{
    public static EqualityConverter Instance { get; } = new();

    public bool Invert { get; set; } = false;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool result = false;

        if (value != null)
        {
            result = value.Equals(parameter);
        }
        else if (parameter != null)
        {
            result = parameter.Equals(value);
        }
        else
        {
            result = true;
        }

        return Invert ? !result : result;
    }

    public object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
