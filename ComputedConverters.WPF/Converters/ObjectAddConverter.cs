using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(double))]
public class ObjectAddConverter : SingletonValueConverterBase<ObjectAddConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            // Assert that `null + any = null`.
            return null;
        }

        return (dynamic)value + parameter;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
