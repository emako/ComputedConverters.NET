using System;

namespace ComputedConverters;

public class ReactiveMapperConfigurationExpression : IReactiveMapperConfigurationExpression
{
    public IReactiveMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        return new ReactiveMappingExpression<TSource, TDestination>();
    }
}

public interface IReactiveMapperConfigurationExpression
{
    public IReactiveMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>();
}
