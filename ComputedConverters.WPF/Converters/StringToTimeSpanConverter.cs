using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts between <see cref="string"/> instances and <see cref="TimeSpan"/> values.
/// </summary>
/// <remarks>
/// When parsing fails, the converter falls back to <c>ConverterParameter</c> (accepting either <see cref="TimeSpan"/> or a parsable string) and ultimately to <c>TimeSpan.Zero</c> for non-nullable targets.
/// </remarks>
[ValueConversion(typeof(string), typeof(TimeSpan))]
public sealed class StringToTimeSpanConverter : SingletonValueConverterBase<StringToTimeSpanConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return value;
        }

        if (TimeSpan.TryParse(value.ToString(), out TimeSpan result))
        {
            return result;
        }

        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        string? format = parameter as string;

        if (value is TimeSpan span)
        {
            return ApplyFormat(span, format, culture);
        }

        TimeSpan? nullableValue = value as TimeSpan?;
        if (nullableValue.HasValue)
        {
            return ApplyFormat(nullableValue.Value, format, culture);
        }

        return value.ToString();
    }

    private static string ApplyFormat(TimeSpan value, string? format, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(format))
        {
            return value.ToString(null, culture);
        }

        return value.ToString(format, culture);
    }
}
