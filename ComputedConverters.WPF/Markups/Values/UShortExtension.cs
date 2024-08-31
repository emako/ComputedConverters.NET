using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(ushort))]
public sealed class UShortExtension(ushort value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public ushort Value { get; set; } = value;

    public UShortExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
