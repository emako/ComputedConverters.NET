# ComputedConverters.NET

ComputedConverters provides you with XAML markup that allows you to write inline converters (Vue-like computed method) and expand some converters commonly used.

## Support framework

WPF (.NET Framewrok and .NET Core)

Avalonia (TBD)

## Usage

Add XML namespace to your XAML file:

```xaml
<Window xmlns:c="https://schemas.elecho.dev/wpfsuite">
    ...
</Window>
```

### Reactivity MVVM

#### Reactive Definition

- Use `ComputedConverters` only.

```c#
public class ViewModel : Reactive
{
}
```

- Use `ComputedConverters` with `CommunityToolkit.Mvvm`.

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject
{
}
```

And also `ReactiveCollection<T>` / `Ref<T>` are availabled.

`ReactiveCollection<T>` is similar to vuejs `reactive(T[])`.

`Ref<T>` is similar to vuejs `ref(T)`.

#### Computed

Computed property that is an instance method of the `ReactiveObject` base class.

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject
{
    [ObservableProperty]
    private double width = 10d;

    [ObservableProperty]
    private double height = 10d;

    public double Area => Computed(() => Width * Height);
}
```

#### Watch

Subscribe to an observable expression and trigger a callback function when its value changes.

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject
{
    public ViewModel()
    {
		Watch(() => Width * Height, area => Debug.WriteLine(area));
    }
}
```

#### WatchDeep

Deep traversal subscribes to an observable object and triggers a callback function when its properties, or the properties of its properties, change.

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject
{
    public ViewModel()
    {
		WatchDeep(obj, path => Debug.WriteLine(path))
    }
}
```

### Value Converters

### Markup Extensions

## Examples

[Test](https://github.com/emako/ComputedConverters.NET/tree/master/ComputedConverters.WPF.Test)

[VSEnc](https://github.com/lemutec/VSEnc)

## Thanks

https://github.com/emako/ComputedConverters.NET/discussions/2

