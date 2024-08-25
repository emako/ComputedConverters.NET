using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public sealed class EqualityConverter : SingletonValueConverterBase<EqualityConverter>
{
    public static readonly DependencyProperty IsInvertedProperty =
        DependencyProperty.Register(nameof(IsInverted), typeof(bool), typeof(EqualityConverter), new PropertyMetadata(false));

    public bool IsInverted
    {
        get => (bool)GetValue(IsInvertedProperty);
        set => SetValue(IsInvertedProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool result = false;

        if (value != null)
        {
            result = value.Equals(parameter);
        }
        else if (parameter != null)
        {
            result = parameter.Equals(value);
        }
        else
        {
            result = true;
        }

        return IsInverted ? !result : result;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
