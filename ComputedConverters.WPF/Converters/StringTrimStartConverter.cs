using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringTrimStartConverter : IValueConverter
{
    public static StringTrimStartConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string trimChars && !string.IsNullOrEmpty(trimChars))
        {
            return value?.ToString()?.TrimStart(trimChars.ToCharArray());
        }
        return value?.ToString()?.TrimStart();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
