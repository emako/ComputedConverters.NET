using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringTrimEndConverter : IValueConverter
{
    public static StringTrimEndConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string trimChars && !string.IsNullOrEmpty(trimChars))
        {
            return value?.ToString()?.TrimEnd(trimChars.ToCharArray());
        }
        return value?.ToString()?.TrimEnd();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
