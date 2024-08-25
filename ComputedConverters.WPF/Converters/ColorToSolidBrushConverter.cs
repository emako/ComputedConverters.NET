﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
public sealed class ColorToSolidBrushConverter : SingletonValueConverterBase<ColorToSolidBrushConverter>
{
    public bool Freeze { get; set; } = true;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            SolidColorBrush brush = new(color);

            if (Freeze)
            {
                brush.Freeze();
            }
            return brush;
        }
        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color;
        }
        return default(Color);
    }
}
