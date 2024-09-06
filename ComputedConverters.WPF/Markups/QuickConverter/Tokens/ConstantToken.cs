using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace ComputedConverters.Tokens;

public class ConstantToken : TokenBase
{
    internal ConstantToken()
    {
    }

    public override Type ReturnType => Value != null ? Value.GetType() : typeof(object);

    public override TokenBase[] Children => [];

    public object Value { get; private set; } = default!;

    internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        token = null!;
        if (text.Length == 0)
        {
            return false;
        }

        if (text[0] == '\'')
        {
            int count = 1;
            while (count < text.Length && !(text[count] == '\'' && text[count - 1] != '\\'))
            {
                ++count;
            }

            if (count > text.Length)
            {
                return false;
            }

            if (text.Length > count + 1 && text[count + 1] == 'c')
            {
                if (count > 2)
                {
                    throw new Exception("The string '" + text.Substring(1, count - 1) + "' can not be interpreted as a character.");
                }

                token = new ConstantToken() { Value = text[1] };
                ++count;
            }
            else
            {
                token = new ConstantToken() { Value = text.Substring(1, count - 1) };
            }

            text = text.Substring(count + 1);
            return true;
        }
        if (text.Length >= 4 && text.Substring(0, 4).ToLower() == "true")
        {
            text = text.Substring(4);
            token = new ConstantToken() { Value = true };
            return true;
        }
        if (text.Length >= 5 && text.Substring(0, 5).ToLower() == "false")
        {
            text = text.Substring(5);
            token = new ConstantToken() { Value = false };
            return true;
        }
        if (text.Length >= 4 && text.Substring(0, 4).ToLower() == "null")
        {
            text = text.Substring(4);
            token = new ConstantToken() { Value = null! };
            return true;
        }
        {
            int count = 0;
            while (count < text.Length && (char.IsDigit(text[count]) || text[count] == '.'))
            {
                ++count;
            }

            if (count > 0 && text[count - 1] == '.')
            {
                --count;
            }

            if (count == 0)
            {
                return false;
            }

            string temp = text.Substring(0, count);
            if (text.Length > count)
            {
                char c = char.ToLower(text[count]);
                ++count;
                object ret = null!;
                if (c == 'u' && text.Length > count + 1 && char.ToLower(text[count]) == 'l')
                {
                    if (ulong.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out ulong val))
                    {
                        ret = val;
                        ++count;
                    }
                }
                else if (c == 'u')
                {
                    if (uint.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out uint val))
                    {
                        ret = val;
                    }
                }
                else if (c == 'l')
                {
                    if (long.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out long val))
                    {
                        ret = val;
                    }
                }
                else if (c == 'f')
                {
                    if (float.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out float val))
                    {
                        ret = val;
                    }
                }
                else if (c == 'd')
                {
                    if (double.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out double val))
                    {
                        ret = val;
                    }
                }
                else if (c == 'm')
                {
                    if (decimal.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out decimal val))
                    {
                        ret = val;
                    }
                }
                if (ret != null)
                {
                    text = text.Substring(count);
                    token = new ConstantToken() { Value = ret };
                    return true;
                }
                --count;
            }
            if (temp.Contains('.'))
            {
                if (double.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out double val1))
                {
                    text = text.Substring(count);
                    token = new ConstantToken() { Value = val1 };
                    return true;
                }
            }
            else if (int.TryParse(temp, NumberStyles.Any, new CultureInfo("en-us"), out int val2))
            {
                text = text.Substring(count);
                token = new ConstantToken() { Value = val2 };
                return true;
            }
        }
        return false;
    }

    internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true)
    {
        return Expression.Constant(Value, typeof(object));
    }
}
