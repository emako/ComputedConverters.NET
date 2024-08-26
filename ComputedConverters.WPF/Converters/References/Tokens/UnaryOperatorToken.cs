using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ComputedConverters.Tokens;

public class UnaryOperatorToken : TokenBase
{
    internal UnaryOperatorToken()
    {
    }

    public override Type ReturnType => typeof(object);

    public override TokenBase[] Children => [value];

    private Operator operation = default;
    private TokenBase value = null!;

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        if (text.Length == 0)
        {
            return false;
        }

        Operator op;
        if (text[0] == '!')
        {
            op = Operator.Not;
        }
        else if (text[0] == '+')
        {
            op = Operator.Positive;
        }
        else if (text[0] == '-')
        {
            op = Operator.Negative;
        }
        else
        {
            return false;
        }

        string temp = text.Substring(1).TrimStart();
        if (!EquationTokenizer.TryGetValueToken(ref temp, out TokenBase valToken))
        {
            return false;
        }

        token = new UnaryOperatorToken() { operation = op, value = valToken };
        text = temp;
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        ExpressionType type = default;
        switch (operation)
        {
            case Operator.Positive:
                return value.GetExpression(parameters, locals, dataContainers, dynamicContext, label);

            case Operator.Negative:
                type = ExpressionType.Negate;
                break;

            case Operator.Not:
                type = ExpressionType.Not;
                break;
        }
        CallSiteBinder binder = Binder.UnaryOperation(CSharpBinderFlags.None, type, dynamicContext ?? typeof(object), [CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)]);
        return Expression.Dynamic(binder, typeof(object), value.GetExpression(parameters, locals, dataContainers, dynamicContext!, label));
    }
}
