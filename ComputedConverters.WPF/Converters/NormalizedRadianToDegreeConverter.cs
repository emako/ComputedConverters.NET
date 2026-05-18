using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts radians to degrees and normalizes the degree result into the [0, 360) range.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public sealed class NormalizedRadianToDegreeConverter : SingletonValueConverterBase<NormalizedRadianToDegreeConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double degree = AngleRadianConverterHelper.RadianToDegree(System.Convert.ToDouble(value, culture));
            double result = AngleRadianConverterHelper.NormalizeDegree(degree);
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
            double degree = AngleRadianConverterHelper.NormalizeDegree(System.Convert.ToDouble(value, culture));
            double result = AngleRadianConverterHelper.DegreeToRadian(degree);
            return AngleRadianConverterHelper.ChangeType(result, targetType, culture);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
