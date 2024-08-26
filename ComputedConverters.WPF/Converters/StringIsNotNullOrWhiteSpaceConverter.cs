using System;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringIsNotNullOrWhiteSpaceConverter : StringIsNullOrWhiteSpaceConverter
{
    private static readonly Lazy<StringIsNotNullOrWhiteSpaceConverter> _instance = new(() => new StringIsNotNullOrWhiteSpaceConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static StringIsNotNullOrWhiteSpaceConverter Instance => _instance.Value;

    public StringIsNotNullOrWhiteSpaceConverter()
    {
        IsInverted = true;
    }
}
