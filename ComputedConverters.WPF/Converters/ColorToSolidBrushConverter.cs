using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputedConverters;

[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
[ValueConversionDynamic("System.Drawing.Color", typeof(SolidColorBrush))]
[ValueConversion(typeof(string), typeof(SolidColorBrush))]
public sealed class ColorToSolidBrushConverter : SingletonValueConverterBase<ColorToSolidBrushConverter>
{
    public bool Freeze { get; set; } = true;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        if (value is Color color)
        {
            if (color == Colors.Transparent)
            {
                return Brushes.Transparent;
            }

            SolidColorBrush brush = new(color);

            if (Freeze)
            {
                brush.Freeze();
            }
            return brush;
        }
        else if (value.GetType().FullName == "System.Drawing.Color")
        {
            dynamic colorDynamic = value;

            if (colorDynamic.A == 0x00 && colorDynamic.R == 0x00 && colorDynamic.G == 0x00 && colorDynamic.B == 0x00)
            {
                return Brushes.Transparent;
            }

            SolidColorBrush brush = new(Color.FromArgb(colorDynamic.A, colorDynamic.R, colorDynamic.G, colorDynamic.B));

            if (Freeze)
            {
                brush.Freeze();
            }
            return brush;
        }
        else if (value is string inputString)
        {
            return ColorConverter.ConvertFromString(inputString);
        }
        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color;
        }
        return default(Color);
    }
}
