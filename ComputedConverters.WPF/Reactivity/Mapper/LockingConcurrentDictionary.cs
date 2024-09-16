using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ComputedConverters;

public readonly struct LockingConcurrentDictionary<TKey, TValue>(Func<TKey, TValue> valueFactory) where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary = new();

    private readonly Func<TKey, Lazy<TValue>> _valueFactory = (TKey key) => new Lazy<TValue>(() => valueFactory(key));

    public TValue this[TKey key]
    {
        get => _dictionary[key].Value;
        readonly set => _dictionary[key] = new Lazy<TValue>(() => value);
    }

    public readonly ICollection<TKey> Keys => _dictionary.Keys;

    public bool TryAdd(TKey key, Lazy<TValue> value)
    {
        return _dictionary.TryAdd(key, value);
    }

    public TValue GetOrAdd(TKey key)
    {
        return _dictionary.GetOrAdd(key, _valueFactory).Value;
    }

    public TValue GetOrAdd(TKey key, Func<TKey, Lazy<TValue>> valueFactory)
    {
        return _dictionary.GetOrAdd(key, valueFactory).Value;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_dictionary.TryGetValue(key, out var value2))
        {
            value = value2.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public TValue GetOrDefault(TKey key)
    {
        TryGetValue(key, out var value);
        return value;
    }
}
