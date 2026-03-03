using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

/// <summary>
/// Inverts an <see cref="IValueConverter"/>: forwards <see cref="Convert"/> to the inner converter's
/// <see cref="IValueConverter.ConvertBack"/>, and <see cref="ConvertBack"/> to the inner converter's
/// <see cref="IValueConverter.Convert"/>.
/// </summary>
/// <remarks>
/// Set <see cref="Converter"/> to the converter to invert. Useful when a binding direction is
/// reversed (e.g. TwoWay) and you want to reuse the same conversion logic.
/// </remarks>
[ContentProperty(nameof(Converter))]
[ValueConversion(typeof(object), typeof(object))]
public sealed class ValueInverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the inner converter whose Convert/ConvertBack are called in reverse.
    /// </summary>
    public IValueConverter? Converter { get; set; }

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Converter == null)
            return Binding.DoNothing;
        return Converter.ConvertBack(value, targetType, parameter, culture);
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Converter == null)
            return Binding.DoNothing;
        return Converter.Convert(value, targetType, parameter, culture);
    }
}
