using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Type), typeof(bool))]
public class TypeToBoolConverter : SingletonValueConverterBase<TypeToBoolConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null && parameter is Type type && value.GetType() == type;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
