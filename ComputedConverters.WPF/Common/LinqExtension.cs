using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputedConverters;

public static class LinqExtension
{
    public static void ForEach<TSource>(this IEnumerable<TSource> ts, Action<TSource> action)
    {
        if (ts == null)
        {
            return;
        }

        foreach (TSource t in ts)
        {
            action(t);
        }
    }

    public static void ForEach<TSource>(this IEnumerable<TSource> ts, Action<TSource, int> action)
    {
        if (ts == null)
        {
            return;
        }

        int i = default;
        foreach (TSource t in ts)
        {
            action(t, i);
            i++;
        }
    }

    public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> ts, Func<TSource, Task> action)
    {
        if (ts == null)
        {
            return;
        }

        foreach (TSource t in ts)
        {
            await action(t);
        }
    }

    public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> ts, Func<TSource, int, Task> action)
    {
        if (ts == null)
        {
            return;
        }

        int i = default;
        foreach (TSource t in ts)
        {
            await action(t, i);
            i++;
        }
    }
}
