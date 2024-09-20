using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(double))]
public sealed class DoubleMaxValueExtension : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return double.MaxValue;
    }
}
