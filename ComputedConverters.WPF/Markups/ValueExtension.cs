using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class ValueExtension(string? value, Type? targetType) : MarkupExtension
{
    [ConstructorArgument("targetType")]
    public Type? TargetType { get; set; } = targetType;

    [ConstructorArgument("value")]
    public string? Value { get; set; } = value;

    public ValueExtension() : this(default, default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (TargetType == null)
        {
            return Value;
        }
        else
        {
            return Convert.ChangeType(Value, TargetType);
        }
    }
}
