using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(IEnumerable<object>))]
public sealed class ValueToEnumerableConverter : SingletonValueConverterBase<ValueToEnumerableConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            return new object[1] { value };
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
