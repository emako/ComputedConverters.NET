# ComputedConverters.NET

ComputedConverters provides you with XAML markup that allows you to write inline converters (Vue-like computed method) and expand some converters commonly used.

## Support framework

WPF (.NET Framewrok and .NET Core)

Avalonia (TBD)

## Usage

Add XML namespace to your XAML file:

```xaml
<Window xmlns:c="http://schemas.lemutec.cn/computedconverters/2024/xaml">
</Window>
```

### 1. Reactivity

Reactivity is a vlue-like MVVM concept.

#### 1.1 Reactive Definition

- Use `ComputedConverters` only.

```c#
public class ViewModel : Reactive
{
}
```

- Recommend: Use `ComputedConverters` with `CommunityToolkit.Mvvm`.

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

### 2. Value Converters

...

### 3. Computed Markup

#### 3.1 Setup

Add the namespaces that it will need to know about (before any xaml loaded).

```c#
public partial class App : Application
{
    public App() : base()
    {
        // Add the System namespace so we can use primitive types (i.e. int, etc.).
        EquationTokenizer.AddNamespace(typeof(object));
        // Add the System.Windows namespace so we can use Visibility.Collapsed, etc.
        EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));
    }
}
```

Other using cases

```c#
EquationTokenizer.AddNamespace("System", Assembly.GetAssembly(typeof(object)));
EquationTokenizer.AddAssembly(typeof(object).Assembly);
EquationTokenizer.AddExtensionMethods(typeof(Enumerable));
```

#### 3.2 Computed

Here is a binding with a Boolean to System.Visibility converter written with ComputedConverter:

```xaml
<Control Visibility="{c:Computed '$P ? Visibility.Visible : Visibility.Collapsed', P={Binding ShowElement}}" />
```

Following are two more examples of valid converter code:

```xaml
'$P != null ? $P.GetType() == typeof(int) : false'
```

```xaml
'(Double.Parse($P) + 123.0).ToString(\\’0.00\\’)'
```

Constructors and object initializers are also valid:

```xaml
<Control FontSize="{c:Computed 'new Dictionary\[string, int\]() { { \\'Sml\\', 16 }, { \\'Lrg\\', 32 } }\[$P\]', P={Binding TestIndex}}" />
```

```xaml
<Control Content="{c:Computed 'new TestObject(1,2,3) { FieldOne = \\'Hello\\', FieldTwo = \\'World\\' }}" />
```

The following shows how to write a two-way binding:

```xaml
<Control Height="{c:Computed '$P * 10', ConvertBack='$value * 0.1', P={Binding TestWidth, Mode=TwoWay}}" />
```

#### 3.3 MultiComputed

Multibinding work in a very similar way.

The following demonstrates an inline multibinding:

```xaml
<Control Angle="{c:MultiComputed 'Math.Atan2($P0, $P1) * 180 / 3.14159', P0={Binding ActualHeight, ElementName=rootElement}, P1={Binding ActualWidth, ElementName=rootElement}}" />
```

#### 3.4 ComputedConverter

Converters can also be created independently of the QuickConverter binding extensions. This allows an extra level of flexibility. The following is an example of this:

```xaml
<Control Width="{Binding Data, Converter={c:ComputedConverter '$P * 10', ConvertBack='$value * 0.1'}}" />
```

**Local Variables**

Local variables can be used through a lambda expression like syntax. Local variables have precedence over binding variables and are only valid with the scope of the lambda expression in which they are declared. The following shows this:

```xaml
<Control IsEnabled="{c:Computed '(Loc = $P.Value, A = $P.Show) => $Loc != null ## $A', P={Binding Obj}}" />
```

\* Note that the "&&" operator must be written as "&amp;&amp;" in XML.

** Due to a bug with the designer, using "&amp;" in a markup extension breaks Intellisense. Instead of two ampersands, use the alternate syntax "##". "#" also works for bitwise and operations.

**Lambda Expressions**

Support for lambda expressions is limited, but its support is sufficient to allow LINQ to be used. They look quite similar to conventional C# lambda expressions, but there are a few important differences. First off, block expressions are not supported. Only single, inline statements are allowed. Also, the expression must return a value. Additionally, the function will default to object for all generic parameters (eg. Func<object, object>). This can be overridden with typecast operators. The following shows this:

```xaml
<Control ItemsSource="{c:Computed '$P.Where(( (int)i ) => (bool)($i % 2 == 0))', P={Binding Source}}" />
```

*Note: The parameters must always be enclosed by parenthesis.

**Null Propagation Operator**

The null propagation operator allows safe property/field, method, and indexer access on objects. When used, if the target object is null, instead of throwing an exception null is returned. The operator is implemented by "?". 

Instead of this:

```xaml
'$P != null ? $P.Value : null'
```

You can write this:

```xaml
'$P?.Value'
```

This can be combined with the null coalescing operator to change this:

```xaml
'$P != null ? $P.GetType() == typeof(int) : false'
```

Into this:

```xaml
'($P?.GetType() == typeof(int)) ?? false'
```

This operator is particularly useful in long statements where there are multiple accessors that could throw null exceptions. In this example, we assume Data can never be null when Value is not null.

```xaml
'$P?.Value?.Data.ArrayData?\[4\]'
```

#### 3.5 ComputedEvent

This markup extension allows you to create event handlers inline. Aside from allowing void functions, the code is identical to QuickConverters. However, QuickEvent exposes a number of variables by default.

```xaml
$sender - The sender of the event.

$eventArgs - The arguments object of the event.

$dataContext - The data context of the sender.

$V0-$V9 - The values set on the QuickEvent Vx properties.

$P0-$P4 - The values of the QuickEvent.P0-QuickEvent.P4 inherited attached properties on sender.

${name} - Any element within the name scope where {name} is the value of x:Name on that element.
```

An example:

```xaml
<StackPanel c:ComputedEvent.P0="{Binding SomeValue}">
   <TextBlock x:Name="textField" />
   <Button Content="Click Me" Click="{c:ComputedEvent '$textField.Text = $dataContext.Transform($P0.Value)'}" />
</StackPanel>
```

The P0-P4 values are useful for crossing name scope boundaries like DataTemplates. Typically, when set outside a DataTemplate they will be inherited by the contents inside the DataTemplate. This allows you to easily make external controls and values accessible from inside DataTemplates.

#### 3.6 ComputedValue

This markup extension evaluates exactly like a ComputedConverter except there are no P0-P9 variables, and it is evaluated at load. The markup extension returns the result of the expression.

### 4. Useful Markup

1. 

2. EventBinding

```xaml
<Window xmlns:c="http://schemas.lemutec.cn/computedconverters/2024/xaml"
        Drop="{c:EventBinding DropCommand}">
</Window>
```

```c#
[RelayCommand]
private void Drop(RelayEventParameter param)
{
    (object sender, DragEventArgs e) = param.Deconstruct<DragEventArgs>();
    // ...
}
```

3. Command

```xaml
<Element Command={markup:Command Execute} />
<Element Command={markup:Command ExecuteWithArgumentAsync, CanExecute}
         CommandParameter={Binding Argument} />
```

```c#
class ViewModel
{
    public void Execute() {}

    public void ExecuteWithArgument(string arg) {}

    // The `Execute` method supports async, and its default `Can Execute` method will disable the command when it is busy.

    public Task ExecuteAsync() => Task.Completed;

    public Task ExecuteWithArgumentAsync(string arg) => Task.Completed;

    // The `Can Execute` method does not support async.

    public bool CanExecute() => true;

    public bool CanExecuteWithArgument(string arg) => true;
}
```

4. IfExtension

Use the `Conditional expression` in XAML.

```xaml
<Button Command="{markup:If {Binding BoolProperty},
                            {Binding OkCommand},
                            {Binding CancelCommand}}" />
```

```xaml
<UserControl>
    <markup:If Condition="{Binding IsLoading}">
        <markup:If.True>
            <views:LoadingView />
        </markup:If.True>
        <markup:If.False>
            <views:LoadedView />
        </markup:If.False>
    </markup:If>
</UserControl>
```

5. SwitchExtension

Use the `Switch expression` in XAML.

```xaml
<Image Source="{markup:Switch {Binding FileType},
                              {Case {x:Static res:FileType.Music}, {StaticResource MusicIcon}},
                              {Case {x:Static res:FileType.Video}, {StaticResource VideoIcon}},
                              {Case {x:Static res:FileType.Picture}, {StaticResource PictureIcon}},
                              ...
                              {Case {StaticResource UnknownFileIcon}}}" />
```

```xaml
<UserControl>
    <Switch To="{Binding SelectedViewName}">
        <Case Label="View1">
            <views:View1 />
        </Case>
        <Case Label="{x:Static res:Views.View2}">
            <views:View2 />
        </Case>
        <Case>
            <views:View404 />
        </Case>
    </Switch>
</UserControl>
```



## Examples

[Test](https://github.com/emako/ComputedConverters.NET/tree/master/ComputedConverters.WPF.Test)

[VSEnc](https://github.com/lemutec/VSEnc)

## Thanks

https://github.com/emako/ComputedConverters.NET/discussions/2

