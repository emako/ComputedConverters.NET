using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleMathRoundConverter : SingletonValueConverterBase<DoubleMathRoundConverter>
{
    public static readonly DependencyProperty DigitsProperty =
        DependencyProperty.Register(nameof(Digits), typeof(int), typeof(DoubleMathRoundConverter), new PropertyMetadata(0));

    public int Digits
    {
        get => (int)GetValue(DigitsProperty);
        set => SetValue(DigitsProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double current = System.Convert.ToDouble(value, culture);
            int digits = Digits;

#if false // Breaking change: No longer supported
            if (parameter is not null)
            {
                digits = System.Convert.ToInt32(parameter, culture);
            }
#endif

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

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
