using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ComputedConverters;

public static class ReactiveMapperAssemblyResolver
{
    private static readonly HashSet<Assembly> stock = [];

    public static void Register(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            StackTrace stackTrace = new(1);
            StackFrame? stackFrame = stackTrace.GetFrame(0);
            MethodBase? methodBase = stackFrame?.GetMethod()!;

            assemblies = [methodBase?.DeclaringType?.Assembly!];
        }

        if (assemblies == null)
        {
            return;
        }

        foreach (Assembly assembly in assemblies)
        {
            if (!stock.Contains(assembly))
            {
                _ = stock.Add(assembly);
            }
            Debug.WriteLine($"[MapperAssemblyResolver] Register assembly named `{assemblies}`.");
        }
    }

    public static IEnumerable<Assembly> Resolve()
    {
        return stock;
    }
}
