using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputedConverters;

public static class LinqExtension
{
    public static void ForEach<TSource>(this IEnumerable<TSource> ts, Action<TSource> action)
    {
        foreach (TSource t in ts)
        {
            action(t);
        }
    }

    public static void ForEach<TSource>(this IEnumerable<TSource> ts, Action<TSource, int> action)
    {
        int i = default;
        foreach (TSource t in ts)
        {
            action(t, i);
            i++;
        }
    }

    public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> ts, Func<TSource, Task> action)
    {
        foreach (TSource t in ts)
        {
            await action(t);
        }
    }

    public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> ts, Func<TSource, int, Task> action)
    {
        int i = default;
        foreach (TSource t in ts)
        {
            await action(t, i);
            i++;
        }
    }
}
