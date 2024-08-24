using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Color))]
public sealed class ColorExtension(object? value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public object? Value { get; set; } = value;

    public ColorExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value switch
        {
            null => Colors.Transparent,
            string => (Color)ColorConverter.ConvertFromString((string)Value),
            Color => (Color)Value,
            SolidColorBrush => ((SolidColorBrush)Value).Color,
            LinearGradientBrush => ((LinearGradientBrush)Value).GradientStops[0].Color,
            RadialGradientBrush => ((RadialGradientBrush)Value).GradientStops[0].Color,
            _ => throw new InvalidOperationException("Value must be a string, Color, SolidColorBrush, LinearGradientBrush or RadialGradientBrush."),
        };
    }
}
