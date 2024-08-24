using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringContainsConverter : SingletonValueConverterBase<StringContainsConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString && parameter is not null)
        {
            return inputString.Contains(parameter.ToString()!);
        }
        return false;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value!;

        if (!isChecked)
        {
            return string.Empty;
        }
        return (parameter as string) ?? string.Empty;
    }
}
