using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ComputedConverters.Tokens;

public class BracketedToken : TokenBase
{
    internal BracketedToken()
    {
    }

    public override Type ReturnType => Value.ReturnType;

    public override TokenBase[] Children => [Value];

    public TokenBase Value { get; private set; } = default!;

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        if (!text.TrimStart().StartsWith("("))
        {
            return false;
        }

        bool inQuotes = false;
        int brackets = 0;
        int i = 0;
        while (true)
        {
            if (i >= text.Length)
            {
                return false;
            }

            if (i > 0 && text[i] == '\'' && text[i - 1] != '\\')
            {
                inQuotes = !inQuotes;
            }
            else if (!inQuotes)
            {
                if (text[i] == '(')
                {
                    ++brackets;
                }
                else if (text[i] == ')')
                {
                    --brackets;
                    if (brackets == 0)
                    {
                        break;
                    }
                }
            }
            ++i;
        }
        if (!EquationTokenizer.TryEvaluateExpression(text.Substring(1, i - 1), out TokenBase valToken))
        {
            return false;
        }

        text = text.Substring(i + 1);
        token = new BracketedToken() { Value = valToken };
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        return Value.GetExpression(parameters, locals, dataContainers, dynamicContext, label);
    }
}
