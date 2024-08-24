using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(string))]
public sealed class StringFormatExtension() : MarkupExtension
{
    public string? Format { get; set; }

    public object? Value { get; set; }

    public object?[]? Values { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Value == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(Format))
        {
            return Format;
        }

        if (Values != null && Values.Length > 0)
        {
            return string.Format(Format!, Values);
        }
        return string.Format(Format!, Value);
    }
}
