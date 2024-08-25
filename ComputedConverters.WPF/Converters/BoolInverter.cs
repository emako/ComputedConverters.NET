using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(bool), typeof(bool))]
public sealed class BoolInverter : SingletonValueConverterBase<BoolInverter>
{
    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(BoolInverter), new PropertyMetadata(true));

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool returnValue = false;

        if (value is bool boolValue)
        {
            if (IsInverted)
            {
                returnValue = !boolValue;
            }
            else
            {
                returnValue = boolValue;
            }
        }

        return returnValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool returnValue = false;

        if (value != null)
        {
            if (IsInverted)
            {
                returnValue = value.Equals(false);
            }
            else
            {
                returnValue = value.Equals(true);
            }
        }

        return returnValue;
    }
}
