using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ComputedConverters;

public static class ReactiveMapperAssemblyResolver
{
    private static readonly HashSet<Assembly> stock = [];

    public static void Register(Assembly? assembly = null)
    {
        if (assembly == null)
        {
            StackTrace stackTrace = new(2);
            StackFrame? stackFrame = stackTrace.GetFrame(1);
            MethodBase? methodBase = stackFrame?.GetMethod()!;

            assembly = methodBase?.DeclaringType?.Assembly;
        }

        if (assembly == null)
        {
            return;
        }

        if (!stock.Contains(assembly))
        {
            _ = stock.Add(assembly);
        }
        Debug.WriteLine($"[MapperAssemblyResolver] Register assembly named `{assembly}`.");
    }

    public static void Unregister(Assembly? assembly = null)
    {
        if (assembly == null)
        {
            StackTrace stackTrace = new(2);
            StackFrame? stackFrame = stackTrace.GetFrame(1);
            MethodBase? methodBase = stackFrame?.GetMethod()!;

            assembly = methodBase?.DeclaringType?.Assembly;
        }

        if (assembly == null)
        {
            return;
        }

        if (stock.Contains(assembly))
        {
            _ = stock.Remove(assembly);
        }
        Debug.WriteLine($"[MapperAssemblyResolver] Unregister assembly named `{assembly}`.");
    }

    public static IEnumerable<Assembly> Resolve()
    {
        return stock;
    }
}
