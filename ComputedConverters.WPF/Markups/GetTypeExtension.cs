using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Type))]
public sealed class GetTypeExtension(object? value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public object? Value { get; set; } = value;

    public GetTypeExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value?.GetType();
    }
}
