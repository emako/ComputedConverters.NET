using ComputedConverters.CalcBinding.ExpressionParsers;
using DynamicExpresso;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace ComputedConverters.CalcBinding.Inversion;

/// <summary>
/// Validate and inverse expression of one parameter
/// </summary>
public class Inverter
{
    private static readonly ExpressionFuncsDictionary<ExpressionType> inversedFuncs = new ExpressionFuncsDictionary<ExpressionType>
    {
        // res = a+c or c+a => a = res - c
        {ExpressionType.Add, ConstantPlace.Wherever, constant => RES + "-" + constant},
        // res = c-a => a = c - res
        {ExpressionType.Subtract, ConstantPlace.Left, constant => constant + "-" + RES},
        // res = a-c => a = res + c
        {ExpressionType.Subtract, ConstantPlace.Right, constant => RES + "+" + constant},
        // res = c*a or a*c => a = res / c
        {ExpressionType.Multiply, ConstantPlace.Wherever, constant => RES + "/" + constant},
        // res = c/a => a = c / res
        {ExpressionType.Divide, ConstantPlace.Left, constant => constant + "/" + RES},
        // res = a/c => a = res*c
        {ExpressionType.Divide, ConstantPlace.Right, constant => RES + "*" + constant},
    };

    private static readonly ExpressionFuncsDictionary<string> inversedMathFuncs = new ExpressionFuncsDictionary<string>
    {
        // res = Math.Sin(a) => a = Math.Asin(res)
        {"Math.Sin", ConstantPlace.Wherever, dummy => "Math.Asin" + RES},
        // res = Math.Asin(a) => a = Math.Sin(res)
        {"Math.Asin", ConstantPlace.Wherever, dummy => "Math.Sin" + RES},

        // res = Math.Cos(a) => a = Math.Acos(res)
        {"Math.Cos", ConstantPlace.Wherever, dummy => "Math.Acos" + RES},
        // res = Math.Acos(a) => a = Math.Cos(res)
        {"Math.Acos", ConstantPlace.Wherever, dummy => "Math.Cos" + RES},

        // res = Math.Tan(a) => a = Math.atan(res)
        {"Math.Tan", ConstantPlace.Wherever, dummy => "Math.Atan" + RES},
        // res = Math.Atan(a) => a = Math.Tan(res)
        {"Math.Atan", ConstantPlace.Wherever, dummy => "Math.Tan" + RES},

        // res = Math.Pow(c, a) => a = Math.Log(res, c)
        {"Math.Pow", ConstantPlace.Left, constant => "Math.Log(" + RES + ", " + constant + ")"},
        // res = Math.Pow(a, c) => a = Math.Pow(res, 1/c)
        {"Math.Pow", ConstantPlace.Right, constant => "Math.Pow(" + RES + ", 1.0/" + constant + ")"},

        // res = Math.Log(c, a) => a = Math.Pow(c, 1/res)
        {"Math.Log", ConstantPlace.Left, constant => "Math.Pow(" + constant + ", 1.0/" + RES + ")"},
        // res = Math.Log(a, c) => a = Math.Pow(c, res)
        {"Math.Log", ConstantPlace.Right, constant => "Math.Pow(" + constant + ", " + RES + ")"},
    };

    public Inverter(IExpressionParser interpreter)
    {
        _interpreter = interpreter;
    }

    /// <summary>
    /// Inverse expression of one parameter
    /// </summary>
    /// <param name="expression">Expression Y=F(X)</param>
    /// <param name="parameter">Type and name of Y parameter</param>
    /// <returns>Inverted expression X = F_back(Y)</returns>
    public Lambda InverseExpression(Expression expression, ParameterExpression parameter)
    {
        RecursiveInfo recInfo = new RecursiveInfo();
        string dummy = null;
        InverseExpressionInternal(expression, recInfo, ref dummy);

        if (recInfo.FoundedParamName == null)
            throw new InverseException(String.Format("Parameter was not found in expression '{0}'!", expression));

        string paramName = parameter.Name;
        string invertedExp = String.Format(recInfo.InvertedExp, paramName);

        Lambda res = _interpreter.Parse(invertedExp, new Parameter(parameter.Name, parameter.Type));
        Debug.WriteLine(res.ExpressionText);
        return res;
    }

    /// <summary>
    /// Generate inversed expression tree from original expression tree of one parameter
    /// using recursion
    /// </summary>
    private NodeType InverseExpressionInternal(Expression expr, RecursiveInfo recInfo, ref string constantExpression)
    {
        switch (expr.NodeType)
        {
            case ExpressionType.Add:
            case ExpressionType.Subtract:
            case ExpressionType.Multiply:
            case ExpressionType.Divide:
                {
                    BinaryExpression? binExp = expr as BinaryExpression;

                    string leftConstant = null!, rightConstant = null!;
                    NodeType leftOperandType = InverseExpressionInternal(binExp!.Left, recInfo, ref leftConstant);
                    NodeType rightOperandType = InverseExpressionInternal(binExp.Right, recInfo, ref rightConstant);

                    NodeType nodeType = (leftOperandType == NodeType.Variable || rightOperandType == NodeType.Variable)
                                    ? NodeType.Variable
                                    : NodeType.Constant;

                    if (nodeType == NodeType.Variable)
                    {
                        ConstantPlace constantPlace = leftOperandType == NodeType.Constant ? ConstantPlace.Left : ConstantPlace.Right;
                        string constant = leftOperandType == NodeType.Constant ? leftConstant : rightConstant;
                        recInfo.InvertedExp = string.Format(recInfo.InvertedExp, inversedFuncs[expr.NodeType, constantPlace](constant));
                    }
                    else
                        constantExpression = string.Format("({0}{1}{2})", leftConstant, NodeTypeToString(binExp.NodeType), rightConstant);

                    return nodeType;
                }
            case ExpressionType.Parameter:
                {
                    ParameterExpression? param = expr as ParameterExpression;

                    if (recInfo.FoundedParamName == null)
                    {
                        recInfo.FoundedParamName = param!.Name!;
                        recInfo.InvertedExp = RES;
                        return NodeType.Variable;
                    }

                    if (recInfo.FoundedParamName == param!.Name)
                        throw new InverseException(string.Format("Variable {0} is defined more than one time!", recInfo.FoundedParamName));
                    else
                        throw new InverseException(string.Format("More than one variables are defined in expression: {0} and {1}", recInfo.FoundedParamName, param.Name));
                }
            case ExpressionType.Constant:
                {
                    ConstantExpression? constant = expr as ConstantExpression;
                    constantExpression = string.Format(CultureInfo.InvariantCulture, "({0})", constant!.Value);
                    return NodeType.Constant;
                }
            case ExpressionType.Convert:
                {
                    UnaryExpression? convertExpr = expr as UnaryExpression;
                    string constant = null!;
                    NodeType operandType = InverseExpressionInternal(convertExpr!.Operand, recInfo, ref constant);

                    if (operandType == NodeType.Constant)
                        constantExpression = "((" + convertExpr.Type.Name + ")" + constant + ")";
                    else
                        recInfo.InvertedExp = String.Format(recInfo.InvertedExp, "((" + convertExpr.Operand.Type.Name + ")" + RES + ")");
                    return operandType;
                }
            case ExpressionType.Negate:
                {
                    UnaryExpression? negateExpr = expr as UnaryExpression;
                    string constant = null!;
                    NodeType operandType = InverseExpressionInternal(negateExpr!.Operand, recInfo, ref constant);

                    if (operandType == NodeType.Constant)
                        constantExpression = "(-" + constant + ")";
                    else
                        recInfo.InvertedExp = String.Format(recInfo.InvertedExp, "(-" + RES + ")");
                    return operandType;
                }
            case ExpressionType.Not:
                {
                    UnaryExpression? convertExpr = expr as UnaryExpression;

                    string constant = null!;
                    NodeType operandType = InverseExpressionInternal(convertExpr!.Operand, recInfo, ref constant);

                    if (operandType == NodeType.Constant)
                        constantExpression = "(" + NodeTypeToString(ExpressionType.Not) + constant + ")";
                    else
                        recInfo.InvertedExp = string.Format(recInfo.InvertedExp, "(" + NodeTypeToString(ExpressionType.Not) + RES + ")");
                    return operandType;
                }
            case ExpressionType.Call:
                {
                    MethodCallExpression? methodExpr = expr as MethodCallExpression;

                    string methodName = methodExpr!.Method!.DeclaringType!.Name + "." + methodExpr.Method.Name;
                    if (!inversedMathFuncs.ContainsKey(methodName))
                    {
                        throw new InverseException(string.Format("Unsupported method call expression: {0}", expr));
                    }

                    string leftConstant = null!, rightConstant = null!;
                    NodeType leftOperandType = InverseExpressionInternal(methodExpr.Arguments[0], recInfo, ref leftConstant);
                    NodeType? rightOperandType = null;

                    if (methodExpr.Arguments.Count == 2)
                    {
                        rightOperandType = InverseExpressionInternal(methodExpr.Arguments[1], recInfo, ref rightConstant);
                    }

                    string inversedRes = null!;
                    if (leftOperandType == NodeType.Variable)
                        inversedRes = inversedMathFuncs[methodName, ConstantPlace.Right](rightConstant);
                    else if (rightOperandType.HasValue && rightOperandType.Value == NodeType.Variable)
                        inversedRes = inversedMathFuncs[methodName, ConstantPlace.Left](leftConstant);
                    else
                    {
                        constantExpression = methodName + "(" + leftConstant;
                        if (rightOperandType != null)
                            constantExpression += ", " + rightConstant;
                        constantExpression += ")";
                    }

                    if (inversedRes != null)
                        recInfo.InvertedExp = string.Format(recInfo.InvertedExp, inversedRes);

                    return inversedRes == null ? NodeType.Constant : NodeType.Variable;
                }
            case ExpressionType.MemberAccess:
                {
                    MemberExpression? memberExpr = expr as MemberExpression;

                    if (memberExpr!.Member.DeclaringType!.Name == "Math")
                    {
                        constantExpression = string.Format(CultureInfo.InvariantCulture, "({0})", memberExpr.Member.DeclaringType.Name + "." + memberExpr.Member.Name);
                        return NodeType.Constant;
                    }
                    else
                    {
                        throw new InverseException(string.Format("Unsupported method call expression: {0}", expr));
                    }
                }
            default:
                throw new InverseException(string.Format("Unsupported expression: {0}", expr));
        }
    }

    private string NodeTypeToString(ExpressionType nodeType)
    {
        switch (nodeType)
        {
            case ExpressionType.Divide:
                return "/";

            case ExpressionType.Multiply:
                return "*";

            case ExpressionType.Subtract:
                return "-";

            case ExpressionType.Add:
                return "+";

            case ExpressionType.Not:
                return "!";

            default:
                throw new Exception("Unknown binary node type: " + nodeType + "!");
        }
    }

    private const string RES = "({0})";

    private IExpressionParser _interpreter;

    #region Types for recursion func work

    internal enum NodeType
    {
        Variable,
        Constant
    }

    internal enum ConstantPlace
    {
        Left,
        Right,
        Wherever
    }

    private class RecursiveInfo
    {
        public string? FoundedParamName;
        public string? InvertedExp;
    }

    private delegate string FuncExpressionDelegate(string constant);

    /// <summary>
    /// Dictionary for inversed funcs static initialize
    /// </summary>
    private class ExpressionFuncsDictionary<T> : Dictionary<T, ConstantPlace, FuncExpressionDelegate>
    {
        public override FuncExpressionDelegate this[T key1, ConstantPlace key2]
        {
            get
            {
                System.Collections.Generic.Dictionary<ConstantPlace, FuncExpressionDelegate> dict = this[key1];

                if (dict.ContainsKey(key2))
                    return dict[key2];

                if (dict.ContainsKey(ConstantPlace.Wherever))
                    return dict[ConstantPlace.Wherever];

                return dict[key2];
            }
        }
    }

    #endregion Types for recursion func work
}
