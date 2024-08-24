using System;
using System.Windows;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class StaticResourceExtension : MarkupExtension
{
    public FrameworkElement? TargetObject { get; set; }
    public object? ResourceKey { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (TargetObject != null)
        {
            return TargetObject.TryFindResource(ResourceKey);
        }
        return Application.Current.TryFindResource(ResourceKey);
    }
}
