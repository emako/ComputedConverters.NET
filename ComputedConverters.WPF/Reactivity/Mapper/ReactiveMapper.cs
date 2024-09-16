using System;

namespace ComputedConverters;

public partial class ReactiveMapper : IReactiveMapper
{
    public TDestination MapFrom<TSource, TDestination>(TSource source, TDestination destination)
    {
        throw new NotImplementedException();
    }

    public TDestination MapFrom<TSource, TDestination>(TSource source)
    {
        throw new NotImplementedException();
    }

    public TDestination MapTo<TSource, TDestination>(TDestination destination, TSource source)
    {
        throw new NotImplementedException();
    }

    public TDestination MapTo<TSource, TDestination>(TDestination destination)
    {
        throw new NotImplementedException();
    }

    public TDestination MapFromCloneable<TSource, TDestination>(TSource source, TDestination destination)
    {
        throw new NotImplementedException();
    }

    public TDestination MapFromCloneable<TSource, TDestination>(TSource source)
    {
        throw new NotImplementedException();
    }

    public TDestination MapToCloneable<TSource, TDestination>(TDestination destination, TSource source)
    {
        throw new NotImplementedException();
    }

    public TDestination MapToCloneable<TSource, TDestination>(TDestination destination)
    {
        throw new NotImplementedException();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IReactiveMapperConfigurationExpression>? configure = null)
    {
        throw new NotImplementedException();
    }
}

public interface IReactiveMapper
{
    public TDestination MapFrom<TSource, TDestination>(TSource source, TDestination destination);

    public TDestination MapFrom<TSource, TDestination>(TSource source);

    public TDestination MapTo<TSource, TDestination>(TDestination destination, TSource source);

    public TDestination MapTo<TSource, TDestination>(TDestination destination);

    public TDestination MapFromCloneable<TSource, TDestination>(TSource source, TDestination destination);

    public TDestination MapFromCloneable<TSource, TDestination>(TSource source);

    public TDestination MapToCloneable<TSource, TDestination>(TDestination destination, TSource source);

    public TDestination MapToCloneable<TSource, TDestination>(TDestination destination);

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IReactiveMapperConfigurationExpression>? configure = null);
}
