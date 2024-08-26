using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ComputedConverters.Tokens;

public class ThrowToken : TokenBase
{
    internal ThrowToken()
    {
    }

    public override Type ReturnType => typeof(object);

    public override TokenBase[] Children => [Exception];

    public TokenBase Exception { get; private set; } = null!;

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        if (!text.StartsWith("throw"))
        {
            return false;
        }

        string temp = text.Substring(5).TrimStart();

        TokenBase valToken = null!;
        if (!EquationTokenizer.TryGetValueToken(ref temp, out valToken))
        {
            return false;
        }

        text = temp;
        token = new ThrowToken() { Exception = valToken };
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        return Expression.Throw(Exception.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(object));
    }
}
