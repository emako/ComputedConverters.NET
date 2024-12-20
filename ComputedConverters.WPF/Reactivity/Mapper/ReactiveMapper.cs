﻿using System;

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
    /// <summary>
    /// Execute a mapping from the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TSource">Source type to use</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object to map from</param>
    /// <param name="destination">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the destination object</returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
        return ReactiveMapperProvider.Service!.Map(source, destination);
    }

    /// <summary>
    /// Execute a mapping from the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TSource">Source type to use</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object to map from</param>
    /// <returns>The mapped destination object, same instance as the destination object</returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source) where TDestination : new()
    {
        return ReactiveMapperProvider.Service!.Map(source, new TDestination());
    }

    /// <summary>
    /// Execute a mapping from the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TSource">Source type to use</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="destination">Destination object to map into</param>
    /// <param name="source">Source object to map from</param>
    /// <returns>The mapped destination object, same instance as the destination object</returns>
    public static TDestination MapFrom<TDestination, TSource>(this TDestination destination, TSource source)
    {
        return ReactiveMapperProvider.Service!.Map(source, destination);
    }

    /// <summary>
    /// Execute a mapping from the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TSource">Source type to use</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="destination">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the destination object</returns>
    public static TDestination MapFrom<TDestination, TSource>(this TDestination destination) where TSource : new()
    {
        return ReactiveMapperProvider.Service!.Map(new TSource(), destination);
    }
}
