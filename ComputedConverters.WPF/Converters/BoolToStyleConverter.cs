using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(bool), typeof(Style))]
public sealed class BoolToStyleConverter : SingletonValueConverterBase<BoolToStyleConverter>
{
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(Style), typeof(BoolToStyleConverter), new PropertyMetadata(default(Style)));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(Style), typeof(BoolToStyleConverter), new PropertyMetadata(default(Style)));

    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(BoolToStyleConverter), new PropertyMetadata(false));

    public object TrueValue
    {
        get => GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public object FalseValue
    {
        get => GetValue(FalseValueProperty);
        set => SetValue(FalseValueProperty, value);
    }

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var returnValue = FalseValue;

        if (value is bool boolValue)
        {
            if (IsInverted)
            {
                returnValue = boolValue ? FalseValue : TrueValue;
            }
            else
            {
                returnValue = boolValue ? TrueValue : FalseValue;
            }
        }

        return returnValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        bool returnValue = false;

        if (value != null)
        {
            if (IsInverted)
            {
                returnValue = value.Equals(FalseValue);
            }
            else
            {
                returnValue = value.Equals(TrueValue);
            }
        }

        return returnValue;
    }
}
