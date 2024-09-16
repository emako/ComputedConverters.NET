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

    public IReactiveMappingExpression<TSource, TDestination> ForAllMembersCloneable()
    {
        throw new NotImplementedException();
        //expression.ValueTransformers.Add<object>(value => (value as ICloneable) != null ? ((ICloneable)value).Clone() : null);
        //return expression;
    }

    public IReactiveMappingExpression<TSource, TDestination> IgnoreAllMembersNull()
    {
        throw new NotImplementedException();
        //foreach (PropertyInfo prop in typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //{
        //    expression.ForMember(prop.Name, opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
        //}
        //return expression;
    }
}

public interface IReactiveMappingExpression<TSource, TDestination>
{
    public void ForAllMembersCustom(Action<TSource, TDestination> function);

    public IReactiveMappingExpression<TSource, TDestination> ForAllMembersCloneable();

    public IReactiveMappingExpression<TSource, TDestination> IgnoreAllMembersNull();
}
