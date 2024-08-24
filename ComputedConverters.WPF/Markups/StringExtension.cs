using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(string))]
public sealed class StringExtension(string? value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public string? Value { get; set; } = value;

    public StringExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
