using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringEqualityConverter : SingletonValueConverterBase<StringEqualityConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString)
        {
            return inputString == parameter as string;
        }
        return value;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value!;

        if (!isChecked)
        {
            return string.Empty;
        }
        return (parameter as string) ?? string.Empty;
    }
}
