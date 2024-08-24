using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(double))]
public sealed class DoubleExtension(double value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public double Value { get; set; } = value;

    public DoubleExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
