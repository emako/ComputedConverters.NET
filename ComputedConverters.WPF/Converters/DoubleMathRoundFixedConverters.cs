using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

internal static class DoubleMathRoundFixedConverterHelper
{
    public static object? Convert(object? value, Type targetType, CultureInfo culture, int digits)
    {
        try
        {
            double current = System.Convert.ToDouble(value, culture);
            double result = Math.Round(current, digits);

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
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound0Converter : SingletonValueConverterBase<DoubleMathRound0Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 0);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound1Converter : SingletonValueConverterBase<DoubleMathRound1Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 1);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound2Converter : SingletonValueConverterBase<DoubleMathRound2Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 2);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound3Converter : SingletonValueConverterBase<DoubleMathRound3Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 3);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound4Converter : SingletonValueConverterBase<DoubleMathRound4Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 4);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound5Converter : SingletonValueConverterBase<DoubleMathRound5Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 5);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRound6Converter : SingletonValueConverterBase<DoubleMathRound6Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DoubleMathRoundFixedConverterHelper.Convert(value, targetType, culture, 6);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
