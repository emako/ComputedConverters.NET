using ComputedConverters.Tokens;

namespace ComputedConverters;

public class TokenizationSuccessEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.TokenizationSuccess;

    public TokenBase Root { get; set; }

    internal TokenizationSuccessEventArgs(string expression, TokenBase root)
        : base(expression)
    {
        Root = root;
    }
}
