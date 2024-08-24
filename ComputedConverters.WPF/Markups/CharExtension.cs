using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(char))]
public sealed class CharExtension(char value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public char Value { get; set; } = value;

    public CharExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
