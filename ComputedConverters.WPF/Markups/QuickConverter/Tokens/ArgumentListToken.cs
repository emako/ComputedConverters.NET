using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ComputedConverters.Tokens;

public class ArgumentListToken : TokenBase
{
    private readonly char open;
    private readonly char close;
    private readonly bool findAssignments;
    private readonly Type assignmentType = null!;
    private readonly bool allowSubLists;
    private readonly bool allowTypeCasts;

    public override Type ReturnType => typeof(object);

    public override TokenBase[] Children => [.. Arguments];

    public TokenBase[] Arguments { get; private set; } = null!;

    internal ArgumentListToken(char open, char close, Type assignmentType)
    {
        this.open = open;
        this.close = close;
        findAssignments = true;
        this.assignmentType = assignmentType;
        allowSubLists = false;
        allowTypeCasts = false;
    }

    internal ArgumentListToken(char open, char close, bool allowSubLists = false)
    {
        this.open = open;
        this.close = close;
        findAssignments = false;
        assignmentType = null!;
        this.allowSubLists = allowSubLists;
        allowTypeCasts = false;
    }

    internal ArgumentListToken(bool allowTypeCasts, char open, char close)
    {
        this.open = open;
        this.close = close;
        findAssignments = false;
        assignmentType = null!;
        allowSubLists = false;
        this.allowTypeCasts = allowTypeCasts;
    }

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        List<TokenBase> list = [];
        string temp = text;
        if (!TrySplitByCommas(ref temp, open, close, out List<string> split))
        {
            return false;
        }

        foreach (string str in split)
        {
            TokenBase newToken;
            string s = str.Trim();
            if (allowSubLists && s.StartsWith(open.ToString()) && s.EndsWith(close.ToString()))
            {
                if (new ArgumentListToken(open, close).TryGetToken(ref s, out newToken))
                {
                    list.Add(newToken);
                }
                else
                {
                    return false;
                }
            }
            else if (findAssignments)
            {
                if (new LambdaAssignmentToken(assignmentType).TryGetToken(ref s, out newToken))
                {
                    list.Add(newToken);
                }
                else
                {
                    return false;
                }
            }
            else if (allowTypeCasts)
            {
                if (new TypeCastToken(false).TryGetToken(ref s, out newToken))
                {
                    string nameTemp = "$" + s;
                    if (!new ParameterToken().TryGetToken(ref nameTemp, out TokenBase tokenTemp) || !string.IsNullOrWhiteSpace(nameTemp))
                    {
                        return false;
                    }
                    (newToken as TypeCastToken)!.Target = tokenTemp;
                    list.Add(newToken);
                }
                else
                {
                    string nameTemp = "$" + s;
                    if (!new ParameterToken().TryGetToken(ref nameTemp, out TokenBase tokenTemp) || !string.IsNullOrWhiteSpace(nameTemp))
                    {
                        return false;
                    }

                    list.Add(tokenTemp);
                }
            }
            else
            {
                if (EquationTokenizer.TryEvaluateExpression(str.Trim(), out newToken))
                {
                    list.Add(newToken);
                }
                else
                {
                    return false;
                }
            }
        }
        token = new ArgumentListToken('\0', '\0') { Arguments = [.. list] };
        text = temp;
        return true;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        throw new NotImplementedException();
    }
}
