using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(IEnumerable), typeof(bool))]
[ValueConversion(typeof(object), typeof(bool))]
public class IsEmptyConverter : SingletonValueConverterBase<IsEmptyConverter>
{
    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(IsEmptyConverter), new PropertyMetadata(false));

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            var hasAtLeastOne = enumerable.GetEnumerator().MoveNext();
            return (hasAtLeastOne == false) ^ IsInverted;
        }

        return true ^ IsInverted;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
