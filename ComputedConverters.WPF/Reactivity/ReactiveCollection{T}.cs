using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ComputedConverters;

public class ReactiveCollection<T> : ObservableCollection<T>
{
    public ReactiveCollection() : base()
    {
    }

    public ReactiveCollection(IEnumerable<T> collection) : base(collection)
    {
    }

    public ReactiveCollection(IList<T> list) : base(list)
    {
    }

    public ReactiveCollection(ICollection<T> collection) : base(collection)
    {
    }

    public ReactiveCollection(IQueryable<T> queryable) : base(queryable)
    {
    }

    protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null!)
    {
        RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    public virtual void AddRange(IEnumerable<T> range)
    {
        foreach (var item in range)
        {
            Items.Add(item);
        }

        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public virtual void Reset(IEnumerable<T> range)
    {
        Items.Clear();
        AddRange(range);
    }

    public virtual void Remove(Func<T, bool> predicate)
    {
        foreach (T item in this.Where(predicate).ToArray())
        {
            Remove(item);
        }
    }
}
