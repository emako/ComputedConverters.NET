using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(decimal))]
public sealed class DecimalExtension(decimal value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public decimal Value { get; set; } = value;

    public DecimalExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
