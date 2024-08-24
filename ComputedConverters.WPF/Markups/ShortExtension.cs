using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(short))]
public sealed class ShortExtension(short value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public short Value { get; set; } = value;

    public ShortExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
