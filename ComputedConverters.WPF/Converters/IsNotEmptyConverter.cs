using System;
using System.Collections;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(IEnumerable), typeof(bool))]
[ValueConversion(typeof(object), typeof(bool))]
public sealed class IsNotEmptyConverter : IsEmptyConverter
{
    private static readonly Lazy<IsNotEmptyConverter> _instance = new(() => new IsNotEmptyConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static IsNotEmptyConverter Instance => _instance.Value;

    public IsNotEmptyConverter()
    {
        IsInverted = true;
    }
}
