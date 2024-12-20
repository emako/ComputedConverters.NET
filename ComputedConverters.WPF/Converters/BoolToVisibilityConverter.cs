﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BoolToVisibilityConverter : SingletonValueConverterBase<BoolToVisibilityConverter>
{
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(Visibility), typeof(BoolToVisibilityConverter), new PropertyMetadata(Visibility.Visible));

    public static readonly DependencyProperty FalseValueProperty =
        DependencyProperty.Register(nameof(FalseValue), typeof(Visibility), typeof(BoolToVisibilityConverter), new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(BoolToVisibilityConverter), new PropertyMetadata(false));

    public Visibility TrueValue
    {
        get => (Visibility)GetValue(TrueValueProperty);
        set => SetValue(TrueValueProperty, value);
    }

    public Visibility FalseValue
    {
        get => (Visibility)GetValue(FalseValueProperty);
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
