using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComputedConverters;

/// <summary>
/// <inheritdoc cref="ReactiveCollection{T}"/>
/// </summary>
public class ReactiveList<T> : BindingList<T>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsEmpty => Count == 0;

    public virtual void AddRange(IEnumerable<T> range)
    {
        foreach (var item in range)
        {
            Items.Add(item);
        }

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEmpty)));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    public virtual void Reset(IEnumerable<T> range)
    {
        Items.Clear();

        AddRange(range);
    }

    protected virtual bool SetProperty(ref T storage, T value, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        RaisePropertyChanged(propertyName);

        return true;
    }

    protected virtual bool SetProperty(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        onChanged?.Invoke();
        RaisePropertyChanged(propertyName);

        return true;
    }

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null!)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }
}
