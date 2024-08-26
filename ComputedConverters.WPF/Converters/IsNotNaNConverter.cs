using System;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(bool))]
public class IsNotNaNConverter : IsNaNConverter
{
    private static readonly Lazy<IsNotNaNConverter> _instance = new(() => new IsNotNaNConverter(), LazyThreadSafetyMode.PublicationOnly);

    public new static IsNotNaNConverter Instance => _instance.Value;

    public IsNotNaNConverter()
    {
        IsInverted = true;
    }
}
