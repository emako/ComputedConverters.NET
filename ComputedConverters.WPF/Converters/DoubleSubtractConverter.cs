﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(double))]
public sealed class DoubleSubtractConverter : SingletonValueConverterBase<DoubleSubtractConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value?.ToString(), NumberStyles.Any, culture, out var basis)
         && double.TryParse(parameter?.ToString(), NumberStyles.Any, culture, out var subtract))
        {
            return basis - subtract;
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}