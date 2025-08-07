namespace ComputedConverters;

public class ReactiveLoader : Reactive
{
    public bool IsInitialized { get; protected set; } = false;

    public void SetInitialized(bool isInitialized)
    {
        IsInitialized = isInitialized;
    }
}
