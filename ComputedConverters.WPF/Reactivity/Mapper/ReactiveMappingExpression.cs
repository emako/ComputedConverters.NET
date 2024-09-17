using System;
using System.Collections.Generic;

namespace ComputedConverters;

public class ReactiveMappingExpression<TSource, TDestination> : IReactiveMappingExpression<TSource, TDestination>
{
    public TypePair TypePair { get; set; } = new(typeof(TSource), typeof(TDestination));

    public Dictionary<TypePair, Action<TSource, TDestination>> _methodCache = [];

    public void ForAllMembersCustom(Action<TSource, TDestination> function)
    {
        _methodCache.Add(TypePair, function);
    }
}

public interface IReactiveMappingExpression<TSource, TDestination>
{
    public void ForAllMembersCustom(Action<TSource, TDestination> function);
}
