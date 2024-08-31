using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(bool))]
public sealed class BoolExtension(bool value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public bool Value { get; set; } = value;

    public BoolExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
