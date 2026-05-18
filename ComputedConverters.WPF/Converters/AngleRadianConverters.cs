using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts degrees to radians, and converts radians back to degrees.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public sealed class DegreeToRadianConverter : SingletonValueConverterBase<DegreeToRadianConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
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

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
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
}

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

/// <summary>
/// Normalizes degrees into the [0, 360) range before converting them to radians.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public sealed class NormalizedDegreeToRadianConverter : SingletonValueConverterBase<NormalizedDegreeToRadianConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
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

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
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
}

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

file static class AngleRadianConverterHelper
{
    /// <summary>
    /// Converts a degree value to radians.
    /// </summary>
    public static double DegreeToRadian(double degree) => degree * Math.PI / 180d;

    /// <summary>
    /// Converts a radian value to degrees.
    /// </summary>
    public static double RadianToDegree(double radian) => radian * 180d / Math.PI;

    /// <summary>
    /// Wraps a degree value into the [0, 360) range.
    /// </summary>
    public static double NormalizeDegree(double degree)
    {
        double result = degree % 360d;
        return result < 0d ? result + 360d : result;
    }

    /// <summary>
    /// Converts the double result to the binding target type when possible.
    /// </summary>
    public static object ChangeType(double value, Type targetType, CultureInfo culture)
    {
        if (typeof(IConvertible).IsAssignableFrom(targetType))
        {
            return System.Convert.ChangeType(value, targetType, culture);
        }

        return value;
    }
}
