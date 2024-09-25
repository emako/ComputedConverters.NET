using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class CastConverter : SingletonValueConverterBase<CastConverter>
{
    public Type? TargetType
    {
        get => (Type?)GetValue(TargetTypeProperty);
        set => SetValue(TargetTypeProperty, value);
    }

    public static readonly DependencyProperty TargetTypeProperty =
        DependencyProperty.Register(nameof(TargetType), typeof(Type), typeof(CastConverter), new PropertyMetadata(null));

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // TODO: Use IL Emit instead of System.Convert.ChangeType.
        return System.Convert.ChangeType(value, TargetType ?? targetType);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
