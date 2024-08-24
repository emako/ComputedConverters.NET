using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class IfExtension() : MarkupExtension
{
    public bool Condition { get; set; } = false;
    public object? TrueValue { get; set; }
    public object? FalseValue { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Condition)
        {
            return TrueValue;
        }
        else
        {
            return FalseValue;
        }
    }
}
