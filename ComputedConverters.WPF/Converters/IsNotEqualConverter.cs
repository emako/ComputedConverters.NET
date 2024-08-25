using System;
using System.Collections;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(IEnumerable), typeof(bool))]
[ValueConversion(typeof(object), typeof(bool))]
public sealed class IsNotEqualConverter : IsEqualConverter
{
    private static readonly Lazy<IsNotEqualConverter> _instance = new(() => new IsNotEqualConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static IsNotEqualConverter Instance => _instance.Value;

    public IsNotEqualConverter()
    {
        IsInverted = true;
    }
}
