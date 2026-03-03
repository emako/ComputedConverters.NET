using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

/// <summary>
/// Converts a string to a <see cref="Color"/> using <see cref="ColorConverter"/>.
/// </summary>
/// <remarks>
/// <para>Supported input formats for <see cref="ColorConverter.ConvertFromString(string)"/>:</para>
/// <list type="bullet">
///   <item>Hex: <c>#rgb</c>, <c>#argb</c>, <c>#rrggbb</c>, <c>#aarrggbb</c> (e.g. <c>#F00</c>, <c>#FF0000</c>, <c>#80FF0000</c>)</item>
///   <item>Named colors: predefined names such as <c>Red</c>, <c>Blue</c>, <c>Transparent</c>, <c>AliceBlue</c> (from <see cref="Colors"/>)</item>
///   <item>ScRGB: <c>sc#scA,scR,scG,scB</c> (e.g. <c>sc#1.0,1.0,0,0</c>)</item>
/// </list>
/// </remarks>
[ValueConversion(typeof(string), typeof(Color))]
public sealed class StringToColorConverter : SingletonValueConverterBase<StringToColorConverter>
{
    /// <inheritdoc />
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string inputString)
        {
            // ColorConverter accepts:
            //   #rgb/#rrggbb/#aarrggbb,
            //   named colors (e.g. "Red"),
            //   or sc#scA,scR,scG,scB
            return ColorConverter.ConvertFromString(inputString);
        }
        return value;
    }

    /// <inheritdoc />
    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            // Include alpha when not fully opaque so round-trip preserves transparency
            return color.A < 255
                ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
        return string.Empty;
    }
}
