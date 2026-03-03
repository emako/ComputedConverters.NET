using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

/// <summary>
/// Converts a string to a <see cref="Brush"/> using <see cref="BrushConverter"/>.
/// </summary>
/// <remarks>
/// <para>Supported input formats for <see cref="BrushConverter.ConvertFromString(string)"/>:</para>
/// <list type="bullet">
///   <item>Hex: <c>#rgb</c>, <c>#argb</c>, <c>#rrggbb</c>, <c>#aarrggbb</c> (e.g. <c>#F00</c>, <c>#FF0000</c>)</item>
///   <item>Named colors: predefined names such as <c>Red</c>, <c>Blue</c>, <c>Transparent</c> (from <see cref="Brushes"/> / <see cref="Colors"/>)</item>
///   <item>ScRGB: <c>sc#scA,scR,scG,scB</c> (e.g. <c>sc#1.0,1.0,0,0</c>)</item>
/// </list>
/// </remarks>
[ValueConversion(typeof(string), typeof(Brush))]
public sealed class StringToBrushConverter : SingletonValueConverterBase<StringToBrushConverter>
{
    /// <inheritdoc />
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString)
        {
            // BrushConverter accepts:
            //   #rgb/#rrggbb/#aarrggbb,
            //   named colors (e.g. "Red"),
            //   or sc#scA,scR,scG,scB
            return new BrushConverter().ConvertFromString(inputString);
        }
        return value;
    }

    /// <inheritdoc />
    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            Color c = brush.Color;

            // Include alpha when not fully opaque so round-trip preserves transparency
            return c.A < 255
                ? $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}"
                : $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }
        return string.Empty;
    }
}
