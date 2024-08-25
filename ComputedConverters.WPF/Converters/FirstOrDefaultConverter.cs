using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(IEnumerable), typeof(object))]
public sealed class FirstOrDefaultConverter : SingletonValueConverterBase<FirstOrDefaultConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            IEnumerator enumerator = enumerable.GetEnumerator();

            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
