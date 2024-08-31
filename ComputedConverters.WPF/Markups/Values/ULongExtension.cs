using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(ulong))]
public sealed class ULongExtension(ulong value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public ulong Value { get; set; } = value;

    public ULongExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
