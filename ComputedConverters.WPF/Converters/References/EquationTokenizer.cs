using ComputedConverters.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ComputedConverters;

public static class EquationTokenizer
{
    private static readonly int[] precedenceLevel = [1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 4, 4, 5, 5, 9, 9, 10, 6, 6, 8, 7, 11];
    internal static readonly string[] representations = ["+", "-", "!", "*", "/", "%", "+", "-", ">=", "<=", ">", "<", "==", "!=", "&&", "##", "||", "&", "#", "|", "^", null!];
    private static Tuple<string, string>[] namespaces = [];
    private static readonly Dictionary<string, Type> types = [];
    private static readonly HashSet<string> assemblies = [];
    private static readonly Dictionary<string, List<MethodInfo>> methods = [];
    private static readonly Dictionary<string, Dictionary<Type, List<MethodInfo>>> methodCache = [];
    private static readonly TokenBase[] valueTypeInstanceList = null!;
    private static readonly TokenBase[] postValueTypeInstanceList = null!;

    internal static bool SupportsExtensionMethods => methods.Count != 0;

    static EquationTokenizer()
    {
        valueTypeInstanceList =
        [
            new StaticFunctionToken(),
            new ConstructorToken(),
            new StaticMemberToken(),
            new ParameterToken(),
            new ConstantToken(),
            new UnaryOperatorToken(),
            new LambdaToken(),
            new BracketedToken(),
            new TypeCastToken(),
            new TypeofToken(),
            new ThrowToken()
        ];
        postValueTypeInstanceList =
        [
            new NullPropagatingToken(),
            new InstanceFunctionToken(),
            new InstanceMemberToken(),
            new IndexerToken(),
            new IsToken(),
            new AsToken(),
            new AssignmentToken()
        ];
        types.Add("bool", typeof(bool));
        types.Add("byte", typeof(byte));
        types.Add("sbyte", typeof(sbyte));
        types.Add("short", typeof(short));
        types.Add("ushort", typeof(ushort));
        types.Add("int", typeof(int));
        types.Add("uint", typeof(uint));
        types.Add("long", typeof(long));
        types.Add("ulong", typeof(ulong));
        types.Add("float", typeof(float));
        types.Add("double", typeof(double));
        types.Add("decimal", typeof(decimal));
        types.Add("char", typeof(char));
        types.Add("string", typeof(string));
        types.Add("object", typeof(object));
        types.Add("bool[]", typeof(bool[]));
        types.Add("byte[]", typeof(byte[]));
        types.Add("sbyte[]", typeof(sbyte[]));
        types.Add("short[]", typeof(short[]));
        types.Add("ushort[]", typeof(ushort[]));
        types.Add("int[]", typeof(int[]));
        types.Add("uint[]", typeof(uint[]));
        types.Add("long[]", typeof(long[]));
        types.Add("ulong[]", typeof(ulong[]));
        types.Add("float[]", typeof(float[]));
        types.Add("double[]", typeof(double[]));
        types.Add("decimal[]", typeof(decimal[]));
        types.Add("char[]", typeof(char[]));
        types.Add("string[]", typeof(string[]));
        types.Add("object[]", typeof(object[]));
    }

    /// <summary>
    /// Adds a namespace for QuickConverter to use when looking up types.
    /// When namespaces span multiple assemblies, each assembly must be added separately.
    /// </summary>
    /// <param name="ns">The namespace to add.</param>
    /// <param name="assembly">The assembly which contains this namespace.</param>
    public static void AddNamespace(string ns, Assembly assembly)
    {
        if (namespaces.Where(tuple => tuple.Item1 == ns).Any(t => t.Item2 == assembly.FullName))
        {
            return;
        }

        namespaces = [.. namespaces, new Tuple<string, string>(ns, assembly.FullName!)];
    }

    /// <summary>
    /// Adds a namespace for QuickConverter to use when looking up types.
    /// When namespaces span multiple assemblies, each assembly must be added separately.
    /// </summary>
    /// <param name="type">The type whose namespace to add.</param>
    public static void AddNamespace(Type type)
    {
        AddNamespace(type.Namespace!, type.Assembly);
    }

    /// <summary>
    /// Adds an assembly to search through when using full type names.
    /// </summary>
    /// <param name="assembly">The assembly to add.</param>
    public static void AddAssembly(Assembly assembly)
    {
        assemblies.Add(assembly.FullName!);
    }

    /// <summary>
    /// Adds the extension methods declared in the specified class.
    /// </summary>
    /// <param name="parentClass">The extension method's parent class.</param>
    public static void AddExtensionMethods(Type parentClass)
    {
        foreach (var method in parentClass.GetMethods().Where(m => m.GetCustomAttributes(typeof(ExtensionAttribute), false).Length != 0))
        {
            if (!methods.TryGetValue(method.Name, out List<MethodInfo>? list))
            {
                list = [];
                methods.Add(method.Name, list);
            }
            list.Add(method);
        }
    }

    static MethodInfo InferTypes(MethodInfo method, params object[] parameters)
    {
        if (method == null || parameters == null || method.GetParameters().Length < parameters.Length)
        {
            return null!;
        }

        Type[] pars = parameters.Select(o => o?.GetType()).ToArray()!;
        Dictionary<Type, Type> arguments = [];
        foreach (Type arg in method.GetGenericArguments())
        {
            arguments.Add(arg, null!);
        }

        ParameterInfo[] info = method.GetParameters();
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (pars[i] == null)
            {
                if (!info[i].ParameterType.IsClass)
                {
                    return null!;
                }
            }
            else if (!info[i].ParameterType.IsGenericType)
            {
                if (arguments.ContainsKey(info[i].ParameterType))
                {
                    if (arguments[info[i].ParameterType] != null)
                    {
                        if (arguments[info[i].ParameterType] != pars[i])
                        {
                            if (pars[i].IsAssignableFrom(arguments[info[i].ParameterType]))
                            {
                                arguments[info[i].ParameterType] = pars[i];
                            }
                            else if (!arguments[info[i].ParameterType].IsAssignableFrom(pars[i]))
                            {
                                return null!;
                            }
                        }
                    }
                    else
                    {
                        arguments[info[i].ParameterType] = pars[i];
                    }
                }
                else if (!info[i].ParameterType.IsAssignableFrom(pars[i]))
                {
                    return null!;
                }
            }
            else
            {
                Type type = GetMatchingType(info[i].ParameterType, pars[i]);
                if (type == null || !MatchTypes(arguments, info[i].ParameterType, type, method))
                {
                    return null!;
                }
            }
        }
        if (arguments.Any(kvp => kvp.Value == null))
        {
            return null!;
        }

        try
        {
            method = method.MakeGenericMethod([.. arguments.Values]);
        }
        catch
        {
            return null!;
        }

        return method;
    }

    static bool MatchTypes(Dictionary<Type, Type> arguments, Type genericType, Type type, MethodInfo method)
    {
        Type[] genericArgs = genericType.GetGenericArguments();
        Type[] args = type.GetGenericArguments();
        for (int i = 0; i < genericArgs.Length; ++i)
        {
            if (genericArgs[i].IsGenericType && !MatchTypes(arguments, genericArgs[i], args[i], method))
            {
                return false;
            }
            else if (arguments.ContainsKey(genericArgs[i]))
            {
                if (arguments[genericArgs[i]] != null)
                {
                    if (arguments[genericArgs[i]] != args[i])
                    {
                        if (args[i].IsAssignableFrom(arguments[genericArgs[i]]))
                        {
                            arguments[genericArgs[i]] = args[i];
                        }
                        else if (!arguments[genericArgs[i]].IsAssignableFrom(args[i]))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    arguments[genericArgs[i]] = args[i];
                }
            }
        }
        return true;
    }

    static Type GetMatchingType(Type genericType, Type type)
    {
        genericType = genericType.GetGenericTypeDefinition();
        Type[] interfaces = type.GetInterfaces();
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return type;
            }

            type = type.BaseType!;
        }
        foreach (Type inter in interfaces)
        {
            if (inter.IsGenericType && inter.GetGenericTypeDefinition() == genericType)
            {
                return inter;
            }
        }
        return null!;
    }

    internal static Tuple<MethodInfo, object[]> GetMethod(string methodName, Type[] typeParams, object[] parameters)
    {
        MethodInfo method = null!;
        if (methods.Count == 0 || parameters[0] == null || !methods.ContainsKey(methodName))
        {
            return null!;
        }

        if (!methodCache.TryGetValue(methodName, out Dictionary<Type, List<MethodInfo>>? typeCache))
        {
            typeCache = [];
            methodCache.Add(methodName, typeCache);
        }
        if (!typeCache.TryGetValue(parameters[0].GetType(), out List<MethodInfo>? methodList))
        {
            methodList = [];
            typeCache.Add(parameters[0].GetType(), methodList);
        }
        Type[] paramTypes = parameters.Select(o => o?.GetType()).ToArray()!;
        if (typeParams != null && typeParams.Length > 0)
        {
            for (int i = 0; i < methodList.Count; ++i)
            {
                var param = methodList[i].GetParameters();
                if (methodList[i].IsGenericMethod && (param.Length == paramTypes.Length || (param.Length > paramTypes.Length && param[paramTypes.Length].IsOptional)))
                {
                    var args = methodList[i].GetGenericArguments();
                    if (args.Length == typeParams.Length)
                    {
                        bool good = true;
                        for (int j = 0; j < args.Length; ++j)
                        {
                            good &= args[j] == typeParams[j];
                        }

                        for (int j = 0; j < paramTypes.Length; ++j)
                        {
                            good &= param[j].ParameterType.IsAssignableFrom(paramTypes[j]) || (param[j].ParameterType.IsClass && paramTypes[j] == null);
                        }

                        if (good)
                        {
                            method = methodList[i];
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < methodList.Count; ++i)
            {
                var param = methodList[i].GetParameters();
                if (param.Length == paramTypes.Length || (param.Length > paramTypes.Length && param[paramTypes.Length].IsOptional))
                {
                    bool good = true;
                    for (int j = 0; j < paramTypes.Length; ++j)
                        good &= param[j].ParameterType.IsAssignableFrom(paramTypes[j]) || (param[j].ParameterType.IsClass && paramTypes[j] == null);
                    if (good)
                    {
                        method = methodList[i];
                        break;
                    }
                }
            }
        }
        if (method == null)
        {
            foreach (MethodInfo meth in methods[methodName])
            {
                if (meth.IsGenericMethod)
                {
                    if (typeParams != null && typeParams.Length > 0)
                    {
                        if (typeParams.Length == meth.GetGenericArguments().Length)
                        {
                            try
                            {
                                method = meth.MakeGenericMethod(typeParams.ToArray());
                            }
                            catch
                            {
                                method = null!;
                            }
                        }
                    }
                    else
                    {
                        method = InferTypes(meth, parameters);
                    }
                }
                else
                {
                    method = meth;
                }

                if (method != null)
                {
                    var param = method.GetParameters();
                    if (param.Length == paramTypes.Length || (param.Length > paramTypes.Length && param[paramTypes.Length].IsOptional))
                    {
                        bool good = true;
                        for (int j = 0; j < paramTypes.Length; ++j)
                        {
                            good &= param[j].ParameterType.IsAssignableFrom(paramTypes[j]) || (param[j].ParameterType.IsClass && paramTypes[j] == null);
                        }

                        if (good)
                        {
                            break;
                        }
                    }
                    method = null!;
                }
            }
            if (method != null)
            {
                methodList.Add(method);
            }
        }
        if (method == null)
        {
            return null!;
        }

        var pars = method.GetParameters();
        if (pars.Length != parameters.Length)
        {
            return new Tuple<MethodInfo, object[]>(method, parameters.Concat(pars.Skip(parameters.Length).Select(p => p.DefaultValue)).ToArray()!);
        }

        return new Tuple<MethodInfo, object[]>(method, parameters);
    }

    internal static bool TryGetType(string name, Type[] typeParams, out Type type)
    {
        if (typeParams != null)
        {
            name += "`" + typeParams.Length;
        }

        if (types.TryGetValue(name, out type!))
        {
            if (typeParams != null)
            {
                try
                {
                    type = type.MakeGenericType(typeParams.ToArray());
                }
                catch
                {
                    type = null!;
                }
            }
            return type != null;
        }
        type = assemblies.Select(s => Type.GetType(name + ", " + s)).FirstOrDefault(t => t != null)!;
        if (type == null)
        {
            Type[] matches = namespaces.Select(str => Type.GetType(str.Item1 + "." + name.Replace('.', '+') + ", " + str.Item2)).Where(t => t != null).ToArray()!;
            if (matches.Length > 1)
            {
                throw new Exception("Ambiguous type found. Could not choose between " + matches.Select(t => t.FullName).Aggregate((s1, s2) => s1 + " and " + s2) + ".");
            }

            if (matches.Length != 0)
            {
                type = matches[0];
            }
        }
        if (type == null)
        {
            return false;
        }

        types.Add(name, type);
        if (typeParams != null)
        {
            try
            {
                type = type.MakeGenericType(typeParams.ToArray());
            }
            catch
            {
                type = null!;
            }
        }
        return type != null;
    }

    internal static bool TryGetValueToken(ref string text, out TokenBase token, bool requireReturnValue = true)
    {
        string temp = null!;
        token = null!;
        foreach (TokenBase type in valueTypeInstanceList)
        {
            temp = text;
            if (type.TryGetToken(ref temp, out token, requireReturnValue))
            {
                break;
            }

            token = null!;
        }
        if (token == null)
        {
            return false;
        }

        text = temp;

        if (token.ReturnType != typeof(void))
        {
            while (true)
            {
                temp = temp.TrimStart();
                bool cont = false;
                foreach (TokenBase type in postValueTypeInstanceList)
                {
                    if (type.TryGetToken(ref temp, out TokenBase newToken, requireReturnValue))
                    {
                        if (newToken.SetPostTarget(token))
                        {
                            token = newToken;
                            text = temp;
                            cont = true;
                            break;
                        }
                    }
                }
                if (!cont)
                {
                    break;
                }
            }

            if (token is IPostToken)
            {
                token = new PostTokenChainToken(token);
            }
        }

        return true;
    }

    internal static bool TryEvaluateExpression(string text, out TokenBase token, bool requireReturnValue = true)
    {
        string temp = text;
        if (new NullCoalesceOperatorToken().TryGetToken(ref temp, out token))
        {
            return true;
        }

        temp = text;
        if (new TernaryOperatorToken().TryGetToken(ref temp, out token))
        {
            return true;
        }

        token = null!;
        List<TokenBase> tokens = [];
        List<Operator> operators = [];
        while (operators.Count == tokens.Count)
        {
            if (tokens.Count != 0 && !requireReturnValue)
            {
                return false;
            }

            if (!TryGetValueToken(ref text, out TokenBase newToken, requireReturnValue))
            {
                return false;
            }

            tokens.Add(newToken);
            text = text.TrimStart();
            for (int i = (int)Operator.Multiply; i <= (int)Operator.BitwiseXor; ++i)
            {
                if (text.StartsWith(representations[i]))
                {
                    operators.Add((Operator)i);
                    text = text.Substring(representations[i].Length).TrimStart();
                    break;
                }
            }
        }
        if (!string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        while (tokens.Count > 1)
        {
            int lastPrecedence = 1000;
            int last = -1;
            for (int i = 0; i < operators.Count; ++i)
            {
                int precendence = precedenceLevel[(int)operators[i]];
                if (precendence < lastPrecedence)
                {
                    lastPrecedence = precendence;
                    last = i;
                }
                else
                {
                    break;
                }
            }
            tokens[last] = new BinaryOperatorToken(tokens[last], tokens[last + 1], operators[last]);
            tokens.RemoveAt(last + 1);
            operators.RemoveAt(last);
        }
        token = tokens[0];
        return true;
    }

    /// <summary>
    /// Tokenizes the given expression into a token tree.
    /// </summary>
    /// <param name="expression">The string to tokenize.</param>
    /// <returns>The resulting root token.</returns>
    public static TokenBase Tokenize(string expression, bool requireReturnValue = true)
    {
        if (!TryEvaluateExpression(expression, out TokenBase token, requireReturnValue))
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("EquationTokenizer Exception (\"" + expression + "\") - Failed to tokenize.");
            }

            ThrowQuickConverterEvent(new TokenizationFailureEventArgs(expression));
            throw new Exception("Failed to tokenize expression \"" + expression + "\". Did you forget a '$'?");
        }
        ThrowQuickConverterEvent(new TokenizationSuccessEventArgs(expression, token));
        return token;
    }

    internal static void ThrowQuickConverterEvent(QuickConverterEventArgs args)
    {
        QuickConverterEvent?.Invoke(args);
    }

    public static event QuickConverterEventHandler QuickConverterEvent;
}
