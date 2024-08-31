using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(float))]
public sealed class FloatExtension(float value) : MarkupExtension
{
    [ConstructorArgument("value")]
    public float Value { get; set; } = value;

    public FloatExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
