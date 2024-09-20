using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(double))]
public sealed class DoubleMinValueExtension : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return double.MinValue;
    }
}
