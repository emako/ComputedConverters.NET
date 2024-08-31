using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(sbyte))]
public sealed class SByteExtension(sbyte value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public sbyte Value { get; set; } = value;

    public SByteExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
