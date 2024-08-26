using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class IfConverter : Reactive, IValueConverter
{
    public static readonly DependencyProperty ConditionProperty =
        DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(IfConverter), new PropertyMetadata(false));

    public static readonly DependencyProperty TrueValueProperty =
    DependencyProperty.Register(nameof(TrueValue), typeof(object), typeof(IfConverter), new PropertyMetadata(default(object)));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(object), typeof(IfConverter), new PropertyMetadata(default(object)));

    public bool Condition
    {
        get => (bool)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    public object? TrueValue
    {
        get => GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public object? FalseValue
    {
        get => GetValue(FalseValueProperty);
        set => SetValue(FalseValueProperty, value);
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Condition)
        {
            if (TrueValue is IValueConverter converter)
            {
                return converter.Convert(value, targetType, parameter, culture);
            }
            return TrueValue;
        }
        else
        {
            if (FalseValue is IValueConverter converter)
            {
                return converter.Convert(value, targetType, parameter, culture);
            }
            return FalseValue;
        }
    }

    public object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
