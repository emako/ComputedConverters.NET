using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(CaseExtension))]
[ContentProperty(nameof(Value))]
public sealed class CaseExtension() : MarkupExtension
{
    internal int Index { get; set; } = SwitchExtension.InvalidIndex;

    [ConstructorArgument(nameof(Label))]
    public object Label { get; set; } = SwitchExtension.DefaultLabel;

    [ConstructorArgument(nameof(Value))]
    public object? Value { get; set; }

    public CaseExtension(object value) : this()
    {
        Value = value;
    }

    public CaseExtension(object option, object value) : this()
    {
        Label = option;
        Value = value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
