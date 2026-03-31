using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRoundConverter : SingletonValueConverterBase<FloatMathRoundConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            float current = System.Convert.ToSingle(value, culture);
            int digits = 0;

            if (parameter is not null)
            {
                digits = System.Convert.ToInt32(parameter, culture);
            }

            float result = (float)Math.Round(current, digits);
            if (typeof(IConvertible).IsAssignableFrom(targetType))
            {
                return System.Convert.ChangeType(result, targetType, culture);
            }

            return result;
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}