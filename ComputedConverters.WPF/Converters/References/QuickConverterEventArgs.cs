namespace ComputedConverters;

public delegate void QuickConverterEventHandler(QuickConverterEventArgs args);

public abstract class QuickConverterEventArgs
{
    public abstract QuickConverterEventType Type { get; }

    public string Expression { get; private set; }

    internal QuickConverterEventArgs(string expression)
    {
        Expression = expression;
    }
}
