using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(int), typeof(bool))]
public sealed class IntToBoolConverter : SingletonValueConverterBase<IntToBoolConverter>
{
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(int), typeof(IntToBoolConverter), new PropertyMetadata(default(int)));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(int), typeof(IntToBoolConverter), new PropertyMetadata(default(int)));

    public static readonly DependencyProperty BaseOnFalseValueProperty =
        DependencyProperty.Register(nameof(BaseOnFalseValue), typeof(bool), typeof(IntToBoolConverter), new PropertyMetadata(false));

    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(IntToBoolConverter), new PropertyMetadata(false));

    public int TrueValue
    {
        get => (int)GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public int FalseValue
    {
        get => (int)GetValue(FalseValueProperty);
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
