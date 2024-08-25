using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public sealed class IsInRangeConverter : SingletonValueConverterBase<IsInRangeConverter>
{
    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(object), typeof(IsInRangeConverter), new PropertyMetadata(defaultValue: null));

    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register(nameof(MinValue), typeof(object), typeof(IsInRangeConverter), new PropertyMetadata(defaultValue: null));

    public object MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public object MinValue
    {
        get => GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IComparable comparable)
        {
            return DependencyProperty.UnsetValue;
        }

        if (MinValue is not IComparable)
        {
            throw new ArgumentException("MinValue must implement IComparable interface", nameof(MinValue));
        }

        if (MaxValue is not IComparable)
        {
            throw new ArgumentException("MaxValue must implement IComparable interface", nameof(MaxValue));
        }

        var minValue = System.Convert.ChangeType(MinValue, comparable.GetType());
        var maxValue = System.Convert.ChangeType(MaxValue, comparable.GetType());

        return (comparable.CompareTo(minValue) >= 0 && comparable.CompareTo(maxValue) <= 0);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
