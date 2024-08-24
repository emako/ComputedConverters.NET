using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(int))]
public sealed class Int32Extension(int value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public int Value { get; set; } = value;

    public Int32Extension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
