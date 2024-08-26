using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public class StringIsNullOrWhiteSpaceConverter : SingletonValueConverterBase<StringIsNullOrWhiteSpaceConverter>
{
    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(StringIsNullOrWhiteSpaceConverter), new PropertyMetadata(false));

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (IsInverted)
        {
            return !string.IsNullOrWhiteSpace(value as string);
        }

        return string.IsNullOrWhiteSpace(value as string);
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
