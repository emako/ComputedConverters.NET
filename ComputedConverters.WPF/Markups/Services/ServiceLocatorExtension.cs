using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public class ServiceLocatorExtension(Type type) : MarkupExtension
{
    [ConstructorArgument(nameof(Type))]
    public Type Type { get; set; } = type;

    public IServiceProvider? ServiceProvider { get; set; } = ComputedServiceProvider.Shared;

    public ServiceLocatorExtension() : this(null!)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type != null)
        {
            return ServiceProvider?.GetService(Type);
        }
        return null;
    }
}
