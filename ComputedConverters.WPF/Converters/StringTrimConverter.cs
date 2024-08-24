using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringTrimConverter : SingletonValueConverterBase<StringTrimConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string trimChars && !string.IsNullOrEmpty(trimChars))
        {
            return value?.ToString()?.Trim(trimChars.ToCharArray());
        }
        return value?.ToString()?.Trim();
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
