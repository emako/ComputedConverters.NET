using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(int))]
public sealed class IntExtension(int value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public int Value { get; set; } = value;

    public IntExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
