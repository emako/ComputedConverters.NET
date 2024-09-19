using System;

namespace ComputedConverters;

public class ReactiveMappingExpression<TSource, TDestination>(ReactiveMapperConfigurationExpression configuration) : IReactiveMappingExpression<TSource, TDestination>
{
    public IReactiveMapperConfigurationExpression Configuration { get; } = configuration;

    public TypePair TypePair { get; set; } = new(typeof(TSource), typeof(TDestination));

    public void ForAllMembersCustom(Action<TSource, TDestination> function)
    {
        Configuration.MethodCache.Add(TypePair, (source, destination) => function((TSource)source, (TDestination)destination));
    }
}

public interface IReactiveMappingExpression<TSource, TDestination>
{
    public TypePair TypePair { get; set; }

    public void ForAllMembersCustom(Action<TSource, TDestination> function);
}
