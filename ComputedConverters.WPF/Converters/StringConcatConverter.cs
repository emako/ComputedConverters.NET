using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringConcatConverter : SingletonValueConverterBase<StringConcatConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string @string && !string.IsNullOrEmpty(@string))
        {
            return value?.ToString() + @string;
        }
        return value?.ToString();
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
