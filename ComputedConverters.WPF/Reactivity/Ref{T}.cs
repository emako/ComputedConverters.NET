namespace ComputedConverters;

public class Ref<T>(T value) : Reactive
{
    protected T value = value;

    public T Value
    {
        get => value;
        set => SetProperty(ref this.value, value);
    }

    public Ref() : this(default!)
    {
    }

    public static implicit operator Ref<T>(T value)
    {
        return new(value);
    }

    public static implicit operator T(Ref<T> value)
    {
        return value.Value;
    }
}
