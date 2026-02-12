#if NET6_0_OR_GREATER
using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts between <see cref="string"/> instances and <see cref="TimeOnly"/> values.
/// </summary>
[ValueConversion(typeof(string), typeof(TimeOnly))]
public sealed class StringToTimeOnlyConverter : SingletonValueConverterBase<StringToTimeOnlyConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return value;
        }

        if (TimeOnly.TryParse(value.ToString(), culture, DateTimeStyles.None, out TimeOnly result))
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

        if (value is TimeOnly time)
        {
            return ApplyFormat(time, format, culture);
        }

        TimeOnly? nullableValue = value as TimeOnly?;
        if (nullableValue.HasValue)
        {
            return ApplyFormat(nullableValue.Value, format, culture);
        }

        return value.ToString();
    }

    private static string ApplyFormat(TimeOnly value, string? format, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(format))
        {
            return value.ToString(null, culture);
        }

        return value.ToString(format, culture);
    }
}
#endif
