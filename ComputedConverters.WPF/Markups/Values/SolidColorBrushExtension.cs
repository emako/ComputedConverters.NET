using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Brush))]
public sealed class SolidColorBrushExtension(object? value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public object? Value { get; set; } = value;

    public bool Freeze { get; set; } = false;

    public SolidColorBrushExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        switch (Value)
        {
            case null:
                return Brushes.Transparent;

            case string:
                {
                    Color color = (Color)ColorConverter.ConvertFromString((string)Value);
                    SolidColorBrush brush = new(color);

                    if (Freeze)
                    {
                        brush.Freeze();
                    }
                    return brush;
                }

            case Color:
                {
                    SolidColorBrush brush = new((Color)Value);

                    if (Freeze)
                    {
                        brush.Freeze();
                    }
                    return brush;
                }

            case Brush:
                return Value;
        }

        throw new InvalidOperationException("Value must be a string, Color, or Brush.");
    }
}
