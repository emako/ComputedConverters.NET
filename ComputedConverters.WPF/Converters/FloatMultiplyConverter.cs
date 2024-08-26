using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(float), typeof(float))]
public sealed class FloatMultiplyConverter : DoubleMultiplyConverter
{
    private static readonly Lazy<FloatMultiplyConverter> _instance = new(() => new FloatMultiplyConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static FloatMultiplyConverter Instance => _instance.Value;

    public new static readonly DependencyProperty ByProperty =
            DependencyProperty.Register(nameof(By), typeof(float), typeof(DoubleMultiplyConverter), new PropertyMetadata(1f));

    public new float By
    {
        get => (float)GetValue(ByProperty);
        set => SetValue(ByProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        object? result = base.Convert(value, targetType, parameter, culture);

        if (result is double doubleResult)
        {
            return (float)doubleResult;
        }
        return result;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        object? result = base.ConvertBack(value, targetType, parameter, culture);

        if (result is double doubleResult)
        {
            return (float)doubleResult;
        }
        return result;
    }
}
