using System;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringIsNotNullOrEmptyConverter : StringIsNullOrEmptyConverter
{
    private static readonly Lazy<StringIsNotNullOrEmptyConverter> _instance = new(() => new StringIsNotNullOrEmptyConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static StringIsNotNullOrEmptyConverter Instance => _instance.Value;

    public StringIsNotNullOrEmptyConverter()
    {
        IsInverted = true;
    }
}
