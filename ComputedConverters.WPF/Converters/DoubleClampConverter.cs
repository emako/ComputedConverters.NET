using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleClampConverter : SingletonValueConverterBase<DoubleClampConverter>
{
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(DoubleClampConverter), new PropertyMetadata(double.NegativeInfinity));

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(DoubleClampConverter), new PropertyMetadata(double.PositiveInfinity));

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double current = System.Convert.ToDouble(value);
            if (current < Minimum)
                current = Minimum;
            if (current > Maximum)
                current = Maximum;

            var result = current;
            if (typeof(IConvertible).IsAssignableFrom(targetType))
            {
                return System.Convert.ChangeType(result, targetType);
            }
            else
            {
                return result;
            }
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
