using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace ComputedConverters.Tokens;

public class AssignmentToken : TokenBase, IPostToken
{
    internal AssignmentToken()
    {
    }

    public override Type ReturnType => Value.ReturnType;

    public override TokenBase[] Children => [Target, Value];

    public TokenBase Target { get; private set; } = default!;

    public TokenBase Value { get; private set; } = default!;

    public Operator Operator { get; private set; } = default;

    internal override bool SetPostTarget(TokenBase target)
    {
        if (target is StaticMemberToken token)
        {
            if (token.Member is PropertyInfo info)
            {
                if (!info.CanWrite)
                {
                    throw new Exception("Static member \"" + info.Name + "\" is readonly and cannot be set.");
                }
                else if (Operator != default && !info.CanRead)
                {
                    throw new Exception("Static member \"" + info.Name + "\" is writeonly and cannot be read.");
                }
            }
            else if (token.Member is not FieldInfo)
            {
                return false;
            }
        }
        else if (target is not InstanceMemberToken)
        {
            return false;
        }

        Target = target;
        if (Operator != default)
        {
            Value = new BinaryOperatorToken(Target, Value, Operator);
        }

        return true;
    }

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        string temp = text;
        var op = default(Operator);
        if (temp.Length < 2)
        {
            return false;
        }

        if (temp[0] != '=')
        {
            if (temp[1] != '=' || temp.Length < 3)
            {
                return false;
            }

            for (int i = (int)Operator.Multiply; i <= (int)Operator.Subtract; ++i)
            {
                if (EquationTokenizer.representations[i][0] == temp[0])
                {
                    op = (Operator)i;
                }
            }
            for (int i = (int)Operator.BitwiseAnd; i <= (int)Operator.BitwiseXor; ++i)
            {
                if (EquationTokenizer.representations[i][0] == temp[0])
                {
                    op = (Operator)i;
                }
            }
            if (op == default)
            {
                return false;
            }
        }

        if (op == default)
        {
            temp = temp.Substring(1).TrimStart();
        }
        else
        {
            temp = temp.Substring(2).TrimStart();
        }

        if (!EquationTokenizer.TryEvaluateExpression(temp, out TokenBase valToken))
        {
            return false;
        }

        text = string.Empty;
        token = new AssignmentToken() { Value = valToken, Operator = op };
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        var value = Value.GetExpression(parameters, locals, dataContainers, dynamicContext, label);
        if (Target is InstanceMemberToken token)
        {
            CallSiteBinder binder = Binder.SetMember(CSharpBinderFlags.None, token.MemberName, dynamicContext ?? typeof(object), [CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)]);
            return Expression.Dynamic(binder, typeof(object), token.Target.GetExpression(parameters, locals, dataContainers, dynamicContext!, label), value);
        }
        else
        {
            var type = (Target as StaticMemberToken)!.Member is PropertyInfo ? ((Target as StaticMemberToken)!.Member as PropertyInfo)!.PropertyType : ((Target as StaticMemberToken)!.Member as FieldInfo)!.FieldType;
            CallSiteBinder binder = Binder.Convert(CSharpBinderFlags.None, type, dynamicContext ?? typeof(object));
            return Expression.Convert(Expression.Assign(Expression.MakeMemberAccess(null, ((StaticMemberToken)Target).Member), Expression.Dynamic(binder, type, value)), typeof(object));
        }
    }
}
