using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string[]), typeof(string))]
public sealed class StringJoinConverter : SingletonValueConverterBase<StringJoinConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        parameter ??= string.Empty;

        if (parameter is not string separator)
        {
            throw new ArgumentException("The parameter must be a string.", nameof(parameter));
        }

        if (value is IEnumerable { } values)
        {
            return string.Join(separator, values.Cast<object>().ToArray());
        }

        return value?.ToString();
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string separator && !string.IsNullOrEmpty(separator))
        {
            return value?.ToString()?.Split(separator.ToCharArray()) ?? [value?.ToString()!];
        }
        return new string[] { value?.ToString()! };
    }
}
