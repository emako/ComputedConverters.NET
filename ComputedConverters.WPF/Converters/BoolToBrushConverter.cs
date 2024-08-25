using System.Globalization;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversion(typeof(bool), typeof(Brush))]
public sealed class BoolToBrushConverter : SingletonValueConverterBase<BoolToBrushConverter>
{
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(Brush), typeof(BoolToBrushConverter), new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(Brush), typeof(BoolToBrushConverter), new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(BoolToBrushConverter), new PropertyMetadata(false));

    public Brush TrueValue
    {
        get => (Brush)GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public Brush FalseValue
    {
        get => (Brush)GetValue(FalseValueProperty);
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
