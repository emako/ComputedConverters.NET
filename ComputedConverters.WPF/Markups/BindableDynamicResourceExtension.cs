using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

/// <summary>
/// <seealso cref="DynamicResourceExtension"/>
/// </summary>
/// <param name="binding"></param>
[MarkupExtensionReturnType(typeof(object))]
public sealed class BindableDynamicResourceExtension(Binding? binding) : MarkupExtension
{
    [ConstructorArgument("resourceKey")]
    public Binding? ResourceKey { get; set; } = binding;

    public BindableDynamicResourceExtension() : this(default)
    {
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (ResourceKey?.Path?.Path is string propName
         && serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget
         && provideValueTarget.TargetObject is DependencyObject targetObject
         && provideValueTarget.TargetProperty is DependencyProperty targetProperty
         && targetObject is FrameworkElement frameworkElement
         && frameworkElement.DataContext != null
         && frameworkElement.DataContext.GetType().GetProperty(propName) is PropertyInfo propInfo)
        {
            object? resourceKey = propInfo.GetValue(frameworkElement.DataContext);

            if (resourceKey != null)
            {
                frameworkElement.SetResourceReference(targetProperty, resourceKey);
            }
            return null!;
        }
        return DependencyProperty.UnsetValue;
    }
}
