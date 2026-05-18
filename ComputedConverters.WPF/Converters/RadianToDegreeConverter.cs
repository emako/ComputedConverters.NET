using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts radians to degrees, and converts degrees back to radians.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public sealed class RadianToDegreeConverter : SingletonValueConverterBase<RadianToDegreeConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double result = AngleRadianConverterHelper.RadianToDegree(System.Convert.ToDouble(value, culture));
            return AngleRadianConverterHelper.ChangeType(result, targetType, culture);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double result = AngleRadianConverterHelper.DegreeToRadian(System.Convert.ToDouble(value, culture));
            return AngleRadianConverterHelper.ChangeType(result, targetType, culture);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
