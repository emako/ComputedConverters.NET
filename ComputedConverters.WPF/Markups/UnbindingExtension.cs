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

    [ConstructorArgument(nameof(Mode))]
    public BindingMode Mode { get; set; } = BindingMode.OneTime;

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
            if (Mode != BindingMode.OneTime)
            {
                throw new InvalidOperationException($"Only `BindingMode.OneTime` Mode supported.");
            }

            if (binding.RelativeSource != null)
            {
                // Not will be thrown nowadays.
                // TODO: Support it.
                _ = new InvalidOperationException($"RelativeSource is not supported.");
            }

            if (binding.ElementName != null)
            {
                if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IRootObjectProvider provideValueTarget)
                {
                    if (provideValueTarget.RootObject is FrameworkElement targetObject)
                    {
                        if (targetObject.FindName(binding.ElementName) is object { } targetElement)
                        {
                            string propertyPath = binding.Path.Path;

                            if (targetElement.GetType().GetProperty(propertyPath) is PropertyInfo propInfo)
                            {
                                return propInfo.GetValue(targetElement);
                            }
                        }
                    }
                }
            }

            if (binding.Source == null)
            {
                // Solved from `DataContext` when the `Source` is null.
                if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IRootObjectProvider provideValueTarget)
                {
                    if (provideValueTarget.RootObject is FrameworkElement targetObject)
                    {
                        if (targetObject.DataContext is null)
                        {
                            // Not support cascading queries for `DataContext`.
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
            else
            {
                string propertyPath = binding.Path.Path;

                if (binding.Source.GetType().GetProperty(propertyPath) is PropertyInfo propInfo)
                {
                    return propInfo.GetValue(binding.Source);
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
