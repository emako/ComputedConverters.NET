using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ComputedConverters.Tokens;

public class IsToken : TokenBase, IPostToken
{
    internal IsToken()
    {
    }

    public override Type ReturnType => typeof(bool);

    public override TokenBase[] Children => [Target];

    public TokenBase Target { get; private set; } = null!;
    public Type Type { get; private set; } = null!;

    internal override bool SetPostTarget(TokenBase target)
    {
        Target = target;
        return true;
    }

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        string temp = text.TrimStart();
        if (!temp.StartsWith("is"))
        {
            return false;
        }

        temp = temp.Substring(2).TrimStart();
        var name = GetNameMatches(temp, null, null).Reverse().FirstOrDefault(tuple => tuple.Item1 is Type);
        if (name == null || (name.Item2.Length != 0 && name.Item2[0] == '.'))
        {
            return false;
        }

        text = name.Item2.TrimStart();
        token = new IsToken() { Type = (name.Item1 as Type)! };
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        return Expression.Convert(Expression.TypeIs(Target.GetExpression(parameters, locals, dataContainers, dynamicContext, label), Type), typeof(object));
    }
}
