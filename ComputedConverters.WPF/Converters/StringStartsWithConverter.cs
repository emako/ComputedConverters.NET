using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringStartsWithConverter : IValueConverter
{
    public static StringStartsWithConverter Instance { get; } = new();

    public StringComparison ComparisonType { get; set; } = StringComparison.OrdinalIgnoreCase;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString && parameter is not null)
        {
            return inputString.StartsWith(parameter.ToString()!, ComparisonType);
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value!;

        if (!isChecked)
        {
            return string.Empty;
        }
        return parameter;
    }
}
