using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(bool))]
public sealed class DoubleToBoolConverter : SingletonValueConverterBase<DoubleToBoolConverter>
{
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(double), typeof(DoubleToBoolConverter), new PropertyMetadata(default(double)));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(double), typeof(DoubleToBoolConverter), new PropertyMetadata(default(double)));

    public static readonly DependencyProperty BaseOnFalseValueProperty =
        DependencyProperty.Register(nameof(BaseOnFalseValue), typeof(bool), typeof(DoubleToBoolConverter), new PropertyMetadata(false));

    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(DoubleToBoolConverter), new PropertyMetadata(false));

    public double TrueValue
    {
        get => (double)GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public double FalseValue
    {
        get => (double)GetValue(FalseValueProperty);
        set => SetValue(FalseValueProperty, value);
    }

    public bool BaseOnFalseValue
    {
        get => (bool)GetValue(BaseOnFalseValueProperty);
        set => SetValue(BaseOnFalseValueProperty, value);
    }

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!BaseOnFalseValue)
        {
            var trueValue = TrueValue;
            return Equals(value, trueValue) ^ IsInverted;
        }

        var falseValue = FalseValue;
        return !Equals(value, falseValue) ^ IsInverted;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        return true.Equals(value) ^ IsInverted ? TrueValue : FalseValue;
    }
}
