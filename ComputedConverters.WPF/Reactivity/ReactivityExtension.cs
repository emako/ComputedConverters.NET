﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ComputedConverters;

public delegate void OnCleanup(Action cleanup);

public delegate void WatchCallback<in T>(T value, T oldValue, OnCleanup onCleanup);

internal static class ReactivityExtension
{
    public static IDisposable Watch<T>(this IReactivity self, Expression<Func<T>> expression, WatchCallback<T> callback)
    {
        Action? cleanup = null;
        void OnCleanup(Action action) => cleanup = action;
        return self.Watch(expression, (value, oldValue) =>
        {
            cleanup?.Invoke();
            callback(value, oldValue, OnCleanup);
        });
    }

    public static IDisposable Watch<T>(this IReactivity self, Expression<Func<T>> expression, Action<T, T> callback)
    {
        Func<T> getter = expression.Compile();
        T oldValue = getter();
        return self.Watch(expression, () =>
        {
            T value = getter();
            callback(value, oldValue);
            oldValue = value;
        });
    }

    public static IDisposable Watch<T>(this IReactivity self, Expression<Func<T>> expression, Action<T> callback)
    {
        Func<T> getter = expression.Compile();
        return self.Watch(expression, () => callback(getter()));
    }

    public static IDisposable WatchEffect<T>(this IReactivity self, Expression<Func<T>> expression, Action callback)
    {
        return self.Watch(expression, () => callback());
    }

    public static IDisposable WatchDeep(this IReactivity self, object target, Action callback)
    {
        return self.WatchDeep(target, _ => callback());
    }

    public static void Run(this Scope scope, Action action)
    {
        using (scope.Begin())
        {
            action?.Invoke();
        }
    }

    internal static bool IsNotify(this Type type)
    {
        return typeof(INotifyPropertyChanged).IsAssignableFrom(type)
            || typeof(INotifyCollectionChanged).IsAssignableFrom(type);
    }
}
