using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(double))]
public class DoubleMultiplyConverter : SingletonValueConverterBase<DoubleMultiplyConverter>
{
    public static readonly DependencyProperty ByProperty =
            DependencyProperty.Register(nameof(By), typeof(double), typeof(DoubleMultiplyConverter), new PropertyMetadata(1d));

    public double By
    {
        get => (double)GetValue(ByProperty);
        set => SetValue(ByProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double current = System.Convert.ToDouble(value);

            if (parameter is not null)
            {
                try
                {
                    current *= System.Convert.ToDouble(parameter);
                }
                catch
                {
                    ///
                }
            }

            var result = current * By;
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

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double current = System.Convert.ToDouble(value);

            if (parameter is not null)
            {
                try
                {
                    current /= System.Convert.ToDouble(parameter);
                }
                catch
                {
                    ///
                }
            }

            var result = current / By;
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
}
