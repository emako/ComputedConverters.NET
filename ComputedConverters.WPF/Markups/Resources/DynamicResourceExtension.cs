using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

/// <summary>
/// <seealso cref="System.Windows.DynamicResourceExtension"/>
/// </summary>
/// <param name="resourceKey"></param>
[MarkupExtensionReturnType(typeof(object))]
public sealed class DynamicResourceExtension(object? resourceKey) : MarkupExtension
{
    [ConstructorArgument(nameof(ResourceKey))]
    public object? ResourceKey { get; set; } = resourceKey;

    private DependencyProperty? targetProperty;

    public DynamicResourceExtension() : this(default)
    {
    }

    /// <returns><see cref="ResourceReferenceExpression"/></returns>
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (ResourceKey is null)
        {
            return DependencyProperty.UnsetValue;
        }
        else if (ResourceKey is Binding binding)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
            {
                if (provideValueTarget.TargetObject is FrameworkElement targetObject)
                {
                    targetProperty = provideValueTarget.TargetProperty as DependencyProperty;

                    if (targetObject.DataContext is null)
                    {
                        // Not available yet
                        // targetObject.DataContextChanged += OnTargetObjectDataContextChanged;
                        return DependencyProperty.UnsetValue;
                    }

                    string propertyPath = binding.Path.Path;

                    if (targetObject.DataContext.GetType().GetProperty(propertyPath) is PropertyInfo propInfo)
                    {
                        object? resourceKey = propInfo.GetValue(targetObject.DataContext);

                        if (binding.Mode == BindingMode.OneTime)
                        {
                            return targetObject.TryFindResource(resourceKey);
                        }
                        else
                        {
                            if (resourceKey != null)
                            {
                                return CreateResourceReferenceExpression(resourceKey);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
            {
                targetProperty = provideValueTarget.TargetProperty as DependencyProperty;

                return CreateResourceReferenceExpression(ResourceKey);
            }
        }

        return DependencyProperty.UnsetValue;
    }

    private void OnTargetObjectDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (ResourceKey is null)
        {
            return;
        }
        else if (ResourceKey is Binding binding)
        {
            FrameworkElement targetObject = (FrameworkElement)sender;

            if (targetObject.DataContext != null)
            {
                targetObject.DataContextChanged -= OnTargetObjectDataContextChanged;

                if (targetProperty != null)
                {
                    string propertyPath = binding.Path.Path;

                    if (targetObject.DataContext.GetType().GetProperty(propertyPath) is PropertyInfo propInfo)
                    {
                        object? resourceKey = propInfo.GetValue(targetObject.DataContext);

                        if (resourceKey != null)
                        {
                            targetObject.SetResourceReference(targetProperty, propertyPath);
                        }
                    }
                }
            }
        }
    }

    private static object? CreateResourceReferenceExpression(object? resourceKey)
    {
        return Activator.CreateInstance(
            typeof(ResourceReferenceExpressionConverter)
                .Assembly
                .GetType("System.Windows.ResourceReferenceExpression")!,
            BindingFlags.Public | BindingFlags.Instance,
            null!,
            [resourceKey],
            null!
        );
    }
}
