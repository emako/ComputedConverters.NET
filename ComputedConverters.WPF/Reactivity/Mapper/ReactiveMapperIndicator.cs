namespace ComputedConverters;

public abstract class ReactiveMapperIndicator : IReactiveMapperIndicator
{
    public abstract void CreateMap(IReactiveMapperConfigurationExpression cfg);
}

public interface IReactiveMapperIndicator
{
    public void CreateMap(IReactiveMapperConfigurationExpression cfg);
}
