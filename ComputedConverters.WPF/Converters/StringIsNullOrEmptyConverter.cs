using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public class StringIsNullOrEmptyConverter : SingletonValueConverterBase<StringIsNullOrEmptyConverter>
{
    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(StringIsNullOrEmptyConverter), new PropertyMetadata(false));

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (IsInverted)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        return string.IsNullOrEmpty(value as string);
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
