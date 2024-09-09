using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
[DefaultValue(null)]
public sealed class SetServiceLocatorExtension : MarkupExtension
{
    /// <summary>
    /// Use `object` to avoid defining `TypeConverter`
    /// /// </summary>
    [ConstructorArgument(nameof(Value))]
    public object? Value
    {
        get => ComputedServiceProvider.Shared;
        set => ComputedServiceProvider.Shared = value as IServiceProvider;
    }

    public SetServiceLocatorExtension() : this(null)
    {
    }

    public SetServiceLocatorExtension(object? value)
    {
        Value = value;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return null;
    }
}
