using System.Threading;

namespace ComputedConverters;

public class DataContainer
{
    private ThreadLocal<object> _value = new();

    public object? Value
    {
        get => _value.Value;
        set => _value.Value = value!;
    }
}
