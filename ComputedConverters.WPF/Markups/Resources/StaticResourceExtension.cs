using System;
using System.Reflection;
using System.Windows.Markup;

namespace ComputedConverters;

/// <summary>
/// <seealso cref="System.Windows.StaticResourceExtension"/>
/// </summary>
/// <param name="resourceKey"></param>
[MarkupExtensionReturnType(typeof(object))]
public class StaticResourceExtension(object? resourceKey) : MarkupExtension
{
    [ConstructorArgument(nameof(ResourceKey))]
    public object? ResourceKey { get; set; } = resourceKey;

    public StaticResourceExtension() : this(default)
    {
    }

    /// <returns><see cref="ResourceReferenceExpression"/></returns>
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        /// Convert <see cref="System.Windows.DynamicResourceExtension"/> to <see cref="System.Windows.StaticResourceExtension"/>
        /// Reflect .NET Internal class <see cref="System.Windows.ResourceReferenceExpression"/>
        if (ResourceKey?.GetType().FullName == "System.Windows.ResourceReferenceExpression")
        {
            if (ResourceKey.GetType().GetProperty("ResourceKey") is PropertyInfo prop)
            {
                ResourceKey = prop.GetValue(ResourceKey);
            }
        }

        return new System.Windows.StaticResourceExtension()
        {
            ResourceKey = ResourceKey,
        }.ProvideValue(serviceProvider);
    }
}
