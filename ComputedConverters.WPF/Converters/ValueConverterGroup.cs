using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[ContentProperty(nameof(Converters))]
public sealed class ValueConverterGroup : IValueConverter
{
    public List<IValueConverter> Converters { get; set; } = [];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Converters is IEnumerable<IValueConverter> converters)
        {
            return converters.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        return DependencyProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Converters is IEnumerable<IValueConverter> converters)
        {
            return converters.Reverse().Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        return DependencyProperty.UnsetValue;
    }
}
