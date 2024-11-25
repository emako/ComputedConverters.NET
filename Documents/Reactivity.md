### 1. Reactivity

Reactivity is a vue-like MVVM concept.

#### 1.1 Reactive Definition

- Use `ComputedConverters` only.

```c#
public class ViewModel : Reactive;
```

- Recommend: Use `ComputedConverters` with `CommunityToolkit.Mvvm`.

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject;
```

Additionally `ReactiveCollection<T>` and `Ref<T>` are availabled.

`ReactiveCollection<T>` is similar to Vue `reactive(T[])`.

`Ref<T>` is similar to Vue `ref(T)`.

#### 1.2 Computed

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

#### 1.3 Watch

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

#### 1.4 WatchDeep

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

#### 1.5 Mapper

IL based auto mapper will auto **shallow** copy the property from source to target object with the same property name.

For more configuration, to set the `ICloneableAttribute`, `ITypeConverterAttribute` and `NotMappedAttribute` for target property attribute.

```c#
TestMapperModel model = new();
TestMapperViewModel viewModel = new();
viewModel = model.MapTo(viewModel); // Auto copy the properties from model to viewModel.
```