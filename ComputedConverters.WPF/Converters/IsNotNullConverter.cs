using System;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public sealed class IsNotNullConverter : IsNullConverter
{
    private static readonly Lazy<IsNotNullConverter> _instance = new(() => new IsNotNullConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static IsNotNullConverter Instance => _instance.Value;

    public IsNotNullConverter()
    {
        IsInverted = true;
    }
}
