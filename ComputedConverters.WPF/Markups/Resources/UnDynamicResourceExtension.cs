using System.Windows.Markup;

namespace ComputedConverters;

/// <inheritdoc/>
[MarkupExtensionReturnType(typeof(object))]
public class UnDynamicResourceExtension(object? resourceKey) : StaticResourceExtension(resourceKey);
