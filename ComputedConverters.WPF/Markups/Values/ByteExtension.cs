using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(byte))]
public sealed class ByteExtension(byte value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public byte Value { get; set; } = value;

    public ByteExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
