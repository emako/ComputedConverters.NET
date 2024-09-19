using System.Collections.Generic;
using System;

namespace ComputedConverters;

public class ReactiveMapperConfigurationExpression : IReactiveMapperConfigurationExpression
{
    public Dictionary<TypePair, Action<object, object>> MethodCache { get; } = [];

    public IReactiveMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        return new ReactiveMappingExpression<TSource, TDestination>(this);
    }
}

public interface IReactiveMapperConfigurationExpression
{
    public Dictionary<TypePair, Action<object, object>> MethodCache { get; }

    public IReactiveMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>();
}
