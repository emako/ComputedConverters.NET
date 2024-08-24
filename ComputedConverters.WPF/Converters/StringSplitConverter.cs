using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string[]))]
public sealed class StringSplitConverter : SingletonValueConverterBase<StringSplitConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string separator && !string.IsNullOrEmpty(separator))
        {
            return value?.ToString()?.Split(separator.ToCharArray()) ?? [value?.ToString()!];
        }
        return new string[] { value?.ToString()! };
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string separator && !string.IsNullOrEmpty(separator) && separator.Length == 1)
        {
            return string.Join(separator, (string[])value!);
        }
        throw new NotImplementedException();
    }
}
