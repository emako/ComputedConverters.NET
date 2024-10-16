﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ComputedConverters;

public class ReactiveObject : DependencyObject
{
    private readonly Dictionary<string, IStrongBox> _cache = [];

    private MulticastDelegate? _propertyChangedDelegate;

    protected internal T Computed<T>(Expression<Func<T>> expression, [CallerMemberName] string propertyName = null!)
    {
        if (!typeof(INotifyPropertyChanged).IsAssignableFrom(GetType()))
        {
            throw new InvalidOperationException($"{GetType().FullName} not implement from INotifyPropertyChanged.");
        }

        if (!_cache.TryGetValue(propertyName, out _))
        {
            _cache.Add(propertyName, new StrongBox<T>(expression.Compile()()));
            Reactivity.Default.Watch(expression, value =>
            {
                StrongBox<T> storage = (StrongBox<T>)_cache[propertyName];
                storage.Value = value;

                if (_propertyChangedDelegate == null)
                {
                    if (typeof(INotifyPropertyChanged).GetEvent(nameof(INotifyPropertyChanged.PropertyChanged)) is EventInfo { } eventInfo)
                    {
                        if (GetType().GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.Instance | BindingFlags.NonPublic) is FieldInfo { } eventField)
                        {
                            if (eventField?.GetValue(this) is MulticastDelegate { } eventDelegate)
                            {
                                _propertyChangedDelegate = eventDelegate;
                            }
                        }
                    }
                }

                PropertyChangedEventArgs eventArgs = new(propertyName);

                foreach (Delegate handler in _propertyChangedDelegate?.GetInvocationList() ?? [])
                {
                    // Notify ui to pull the latest value after updating the storage.
                    handler.DynamicInvoke(this, eventArgs);
                }
            });
        }

        return ((StrongBox<T>)_cache[propertyName]).Value!;
    }

    protected internal static void Watch<T>(Expression<Func<T>> expression, Action<T> callback)
    {
        Reactivity.Default.Watch(expression, callback);
    }

    protected internal static void WatchEffect<T>(Expression<Func<T>> expression, Action callback)
    {
        Reactivity.Default.WatchEffect(expression, callback);
    }

    protected internal static void WatchDeep<T>(object target, Action<string> callback)
    {
        Reactivity.Default.WatchDeep(target, callback);
    }
}
