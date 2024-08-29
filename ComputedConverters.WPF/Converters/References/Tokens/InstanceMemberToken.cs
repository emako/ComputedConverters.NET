using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ComputedConverters.Tokens;

public class InstanceMemberToken : TokenBase, IPostToken
{
    internal InstanceMemberToken()
    {
    }

    public override Type ReturnType => typeof(object);

    public override TokenBase[] Children => [Target];

    public string MemberName { get; private set; } = null!;
    public TokenBase Target { get; private set; } = null!;

    internal override bool SetPostTarget(TokenBase target)
    {
        Target = target;
        return true;
    }

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        string temp = text;
        if (temp.Length < 2 || temp[0] != '.' || (!char.IsLetter(temp[1]) && temp[1] != '_'))
        {
            return false;
        }

        int count = 2;
        while (count < temp.Length && (char.IsLetterOrDigit(temp[count]) || temp[count] == '_'))
        {
            ++count;
        }

        if (count < temp.Length && temp[count] == '(')
        {
            return false;
        }

        string name = temp.Substring(1, count - 1);
        text = temp.Substring(count);
        token = new InstanceMemberToken() { MemberName = name };
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        CallSiteBinder binder = Binder.GetMember(CSharpBinderFlags.None, MemberName, dynamicContext ?? typeof(object), [CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)]);
        return Expression.Dynamic(binder, typeof(object), Target.GetExpression(parameters, locals, dataContainers, dynamicContext!, label));
    }
}
