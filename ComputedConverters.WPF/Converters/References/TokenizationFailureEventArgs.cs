namespace ComputedConverters;

public class TokenizationFailureEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.TokenizationFailure;

    internal TokenizationFailureEventArgs(string expression) : base(expression)
    {
    }
}
