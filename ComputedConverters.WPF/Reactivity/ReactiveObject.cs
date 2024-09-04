using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ComputedConverters;

public class ReactiveObject : DependencyObject
{
    private readonly IDictionary<string, IStrongBox> _cache = new Dictionary<string, IStrongBox>();

    private MulticastDelegate? _propertyChangedDelegate;

    protected T Computed<T>(Expression<Func<T>> expression, [CallerMemberName] string propertyName = null!)
    {
        if (!typeof(INotifyPropertyChanged).IsAssignableFrom(GetType()))
        {
            throw new InvalidOperationException($"{GetType().FullName} not implement from INotifyPropertyChanged.");
        }

        if (!_cache.ContainsKey(propertyName))
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

    protected static void Watch<T>(Expression<Func<T>> expression, Action<T> callback)
    {
        Reactivity.Default.Watch(expression, callback);
    }

    protected static void WatchDeep<T>(object target, Action<string> callback)
    {
        Reactivity.Default.WatchDeep(target, callback);
    }
}
