using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class CastExtension(object? value, Type? targetType) : MarkupExtension
{
    [ConstructorArgument("value")]
    public object? Value { get; set; } = value;

    [ConstructorArgument("targetType")]
    public Type? TargetType { get; set; } = targetType;

    public CastExtension(object? value) : this(value, default)
    {
    }

    public CastExtension() : this(default, default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Value == null)
        {
            return null;
        }

        if (TargetType == null)
        {
            return Value;
        }

        return Convert.ChangeType(Value, TargetType);
    }
}
