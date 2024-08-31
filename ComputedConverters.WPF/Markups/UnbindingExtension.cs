using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class UnbindingExtension(object? resourceKey) : MarkupExtension
{
    [ConstructorArgument(nameof(ResourceKey))]
    public object? ResourceKey { get; set; } = resourceKey;

    public UnbindingExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (ResourceKey is null)
        {
            return DependencyProperty.UnsetValue;
        }
        else if (ResourceKey is Binding binding)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IRootObjectProvider provideValueTarget)
            {
                if (provideValueTarget.RootObject is FrameworkElement targetObject)
                {
                    if (targetObject.DataContext is null)
                    {
                        return DependencyProperty.UnsetValue;
                    }

                    string propertyPath = binding.Path.Path;

                    if (targetObject.DataContext.GetType().GetProperty(propertyPath) is PropertyInfo propInfo)
                    {
                        return propInfo.GetValue(targetObject.DataContext);
                    }
                }
            }
        }
        else if (ResourceKey is MarkupExtension markup)
        {
            return markup.ProvideValue(serviceProvider);
        }

        return DependencyProperty.UnsetValue;
    }
}
