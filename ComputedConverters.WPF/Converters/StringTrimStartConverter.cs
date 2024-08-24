using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringTrimStartConverter : SingletonValueConverterBase<StringTrimStartConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string trimChars && !string.IsNullOrEmpty(trimChars))
        {
            return value?.ToString()?.TrimStart(trimChars.ToCharArray());
        }
        return value?.ToString()?.TrimStart();
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
