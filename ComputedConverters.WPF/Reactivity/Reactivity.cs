using System;
using System.Linq;
using System.Linq.Expressions;

namespace ComputedConverters;

public class Reactivity : IReactivity
{
    public static IReactivity Default { get; } = new Reactivity();

    public IDisposable Watch<T>(Expression<Func<T>> expression, Action callback)
    {
        DependencyGraph graph = GenerateDependencyGraph(expression);
        IDisposable[] tokens = graph.DependencyRootNodes
            .Select(item => item.Initialize(callback))
            .Concat(graph.ConditionalRootNodes.Select(item => item.Initialize()))
            .ToArray();

        IDisposable token = Disposable.Create(() =>
        {
            foreach (IDisposable token in tokens)
            {
                token.Dispose();
            }
        });

        ComputedConverters.Scope.ActiveEffectScope?.AddStopToken(token);
        return token;
    }

    public IDisposable WatchDeep(object target, Action<string> callback)
    {
        var nodes = DeepNode.Create(target, string.Empty);
        foreach (var item in nodes)
        {
            item.Subscribe(callback);
        }

        IDisposable token = Disposable.Create(() =>
        {
            foreach (var item in nodes)
            {
                item.Unsubscribe();
            }
        });

        ComputedConverters.Scope.ActiveEffectScope?.AddStopToken(token);
        return token;
    }

    public Scope Scope(bool detached = false) => new(detached);

    private static DependencyGraph GenerateDependencyGraph<T>(Expression<Func<T>> expression)
    {
        var visitor = new SingleLineLambdaVisitor();
        visitor.Visit(expression);
        return new DependencyGraph(visitor.RootNodes, visitor.ConditionalNodes);
    }
}

public interface IReactivity
{
    public IDisposable Watch<T>(Expression<Func<T>> expression, Action callback);

    public IDisposable WatchDeep(object target, Action<string> callback);

    public Scope Scope(bool detached = false);
}
