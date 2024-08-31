using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(uint))]
public sealed class UIntExtension(uint value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public uint Value { get; set; } = value;

    public UIntExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
