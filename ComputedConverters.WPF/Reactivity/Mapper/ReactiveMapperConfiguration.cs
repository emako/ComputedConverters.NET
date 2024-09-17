using System;

namespace ComputedConverters;

public class ReactiveMapperConfiguration
{
    public ReactiveMapperConfiguration(Action<IReactiveMapperConfigurationExpression> configure) : this(Build(configure))
    {
    }

    public ReactiveMapperConfiguration(ReactiveMapperConfigurationExpression configurationExpression)
    {
    }

    private static ReactiveMapperConfigurationExpression Build(Action<IReactiveMapperConfigurationExpression> configure)
    {
        ReactiveMapperConfigurationExpression mapperConfigurationExpression = new();
        configure(mapperConfigurationExpression);
        return mapperConfigurationExpression;
    }

    public void AssertConfigurationIsValid()
    {
        // TODO
    }

    public IReactiveMapper CreateMapper()
    {
        return new ReactiveMapper();
    }
}
