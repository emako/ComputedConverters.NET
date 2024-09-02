using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Brush))]
public class AlternatingGridColorExtension : MarkupExtension
{
    public virtual Color ColorEven { get; set; }

    public virtual Color ColorOdd { get; set; }

    public bool Freeze { get; set; } = false;

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        SolidColorBrush brush = new(ColorEven);

        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service)
        {
            if (service.TargetObject is DependencyObject depObject)
            {
                int row = (int)depObject.GetValue(Grid.RowProperty);

                if (row % 2 != 0)
                {
                    brush.Color = ColorOdd;
                }
            }
        }

        if (Freeze)
        {
            brush.Freeze();
        }

        return brush;
    }
}
