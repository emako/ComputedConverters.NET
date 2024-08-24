using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(uint))]
public sealed class UInt32Extension(uint value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public uint Value { get; set; } = value;

    public UInt32Extension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
