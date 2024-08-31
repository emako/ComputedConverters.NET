using System;
using System.Reflection;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(bool))]
public sealed class LogicalNotExtension(object? value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public object? Value { get; set; } = value;

    public LogicalNotExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Value is bool boolValue)
        {
            return !boolValue;
        }
        if (Value is not null)
        {
            MethodInfo? logicalNotMethod = Value.GetType()
                .GetMethod("op_LogicalNot", BindingFlags.Static | BindingFlags.Public);

            if (logicalNotMethod != null)
            {
                return logicalNotMethod.Invoke(null, [Value]);
            }
        }

        return Value;
    }
}
