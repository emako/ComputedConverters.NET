using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(bool))]
public sealed class DoubleEqualsConverter : SingletonValueConverterBase<DoubleEqualsConverter>
{
    private const double Epsilon = 1E-6;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value?.ToString(), NumberStyles.Any, culture, out var basis)
         && double.TryParse(parameter?.ToString(), NumberStyles.Any, culture, out var subtract))
        {
            return Math.Abs(basis - subtract) < Epsilon;
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
