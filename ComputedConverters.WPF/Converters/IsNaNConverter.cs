using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(bool))]
public sealed class IsNaNConverter : SingletonValueConverterBase<IsNaNConverter>
{
    public bool Invert { get; set; } = false;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool result = false;

        if (value is double { })
        {
            result = double.IsNaN((double)value);
        }
        return Invert ? !result : result;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
