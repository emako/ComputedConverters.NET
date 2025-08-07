using System.Collections.Generic;
using System.Linq;

namespace ComputedConverters;

public class ReactiveCollectionLoader<T> : ReactiveCollection<T>
{
    public bool IsInitialized { get; protected set; } = false;

    public ReactiveCollectionLoader() : base()
    {
    }

    public ReactiveCollectionLoader(IEnumerable<T> collection) : base(collection)
    {
    }

    public ReactiveCollectionLoader(IList<T> list) : base(list)
    {
    }

    public ReactiveCollectionLoader(ICollection<T> collection) : base(collection)
    {
    }

    public ReactiveCollectionLoader(IQueryable<T> queryable) : base(queryable)
    {
    }

    public void SetInitialized(bool isInitialized)
    {
        IsInitialized = isInitialized;
    }
}
