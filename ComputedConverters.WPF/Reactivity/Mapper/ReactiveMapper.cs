using System;

namespace ComputedConverters;

public partial class ReactiveMapper : IReactiveMapper
{
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (MethodCache.TryGetValue(new(typeof(TSource), typeof(TDestination)), out Action<object?, object?> method))
        {
            method.Invoke(source, destination);
            return destination;
        }
        else
        {
            return PropertyCopier.Map(source, destination);
        }
    }
}

public interface IReactiveMapper
{
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}

public static class ReactiveMapperExtension
{
    public static TDestination MapFrom<TSource, TDestination>(this TSource source, TDestination destination)
    {
        return ReactiveMapperProvider.Service!.Map(source, destination);
    }

    public static TDestination MapFrom<TSource, TDestination>(this TSource source)
    {
        return ReactiveMapperProvider.Service!.Map(source, Activator.CreateInstance<TDestination>());
    }

    public static TSource MapTo<TSource, TDestination>(this TDestination destination, TSource source)
    {
        return ReactiveMapperProvider.Service!.Map(destination, source);
    }

    public static TSource MapTo<TSource, TDestination>(this TDestination destination)
    {
        return ReactiveMapperProvider.Service!.Map(destination, Activator.CreateInstance<TSource>());
    }
}
