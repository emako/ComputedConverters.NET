using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(ImageSource))]
public sealed class ImageSourceExtension(object? value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public object? Value { get; set; } = value;

    public int? DecodePixelWidth { get; set; } = null;

    public ImageSourceExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return ImageExtension.ToBitmapSource(Value, DecodePixelWidth);
    }
}
