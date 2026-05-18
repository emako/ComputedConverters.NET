using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

internal static class FloatMathRoundFixedConverterHelper
{
    public static object? Convert(object? value, Type targetType, CultureInfo culture, int digits)
    {
        try
        {
            float current = System.Convert.ToSingle(value, culture);
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
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound0Converter : SingletonValueConverterBase<FloatMathRound0Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 0);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound1Converter : SingletonValueConverterBase<FloatMathRound1Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 1);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound2Converter : SingletonValueConverterBase<FloatMathRound2Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 2);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound3Converter : SingletonValueConverterBase<FloatMathRound3Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 3);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound4Converter : SingletonValueConverterBase<FloatMathRound4Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 4);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound5Converter : SingletonValueConverterBase<FloatMathRound5Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 5);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMathRound6Converter : SingletonValueConverterBase<FloatMathRound6Converter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return FloatMathRoundFixedConverterHelper.Convert(value, targetType, culture, 6);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
