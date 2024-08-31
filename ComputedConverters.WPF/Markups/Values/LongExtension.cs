using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(long))]
public sealed class LongExtension(long value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public long Value { get; set; } = value;

    public LongExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
