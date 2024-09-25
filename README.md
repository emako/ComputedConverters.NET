[![GitHub license](https://img.shields.io/github/license/emako/ComputedConverters.NET)](https://github.com/emako/ComputedConverters.NET/blob/master/LICENSE.txt) [![Actions](https://github.com/emako/ComputedConverters.NET/actions/workflows/library.nuget.yml/badge.svg)](https://github.com/emako/ComputedConverters.NET/actions/workflows/library.nuget.yml)

# ComputedConverters.NET

ComputedConverters provides you with XAML markup that allows you to write inline converters (Vue-like computed method) and expand some converters commonly used.

## Support framework

|                    | WPF                                                          | Avalonia                                                     |
| ------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ |
| ComputedConverters | ComputedConverters.WPF [![NuGet](https://img.shields.io/nuget/v/ComputedConverters.WPF.svg)](https://nuget.org/packages/ComputedConverters.WPF) | ComputedConverters.Avalonia (TBD)                            |
| ComputedAnimations | ComputedAnimations.WPF [![NuGet](https://img.shields.io/nuget/v/ComputedAnimations.WPF.svg)](https://nuget.org/packages/ComputedAnimations.WPF) | /                                                            |
| ComputedBehaviors  | ComputedBehaviors.WPF [![NuGet](https://img.shields.io/nuget/v/ComputedBehaviors.WPF.svg)](https://nuget.org/packages/ComputedBehaviors.WPF) | /                                                            |
| ValueConverters    | ValueConverters [![Version](https://img.shields.io/nuget/v/ValueConverters.svg)](https://www.nuget.org/packages/ValueConverters) | ValueConverters.Avalonia [![Version](https://img.shields.io/nuget/v/ValueConverters.Avalonia.svg)](https://www.nuget.org/packages/ValueConverters.Avalonia) |

## Usage

Add XML namespace to your XAML file:

```xaml
<Application xmlns:a="http://schemas.github.com/computedanimations/2024/xaml"
      xmlns:b="http://schemas.github.com/computedbehaviors/2024/xaml"
      xmlns:c="http://schemas.github.com/computedconverters/2024/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <a:ComputedAnimationsResources />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

> [!TIP]
>
> Sample code is not fully introduced, more features please pay more attention to the source code!

### 1. Reactivity

Reactivity is a vue-like MVVM concept.

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

IL based auto mapper will auto copy the property from source to target object with the same property name.

For more configuration, to set the `ICloneableAttribute`, `ITypeConverterAttribute` and `NotMappedAttribute` for target property attribute.

```c#
TestMapperModel model = new();
TestMapperViewModel viewModel = new();
viewModel = model.MapTo(viewModel); // Auto copy the properties from model to viewModel.
```

### 2. Value Converters

#### 2.1 ValueConverterGroup

Supports continuous value converters from group.

```xaml
<c:ValueConverterGroup x:Key="NullToVisibilityConverter">
    <c:IsNullConverter />
    <c:BoolToVisibilityConverter />
</c:ValueConverterGroup>
```

#### 2.2 BoolToVisibilityConverter

```xaml
<TextBlock
  xmlns:diagnostics="clr-namespace:System.Diagnostics;assembly=System.Runtime"
  Text="Visiable on debugger attached."
  Visibility="{c:Converter Value={x:Static diagnostics:Debugger.IsAttached},
                           Converter={x:Static c:BoolToVisibilityConverter.Instance}}" />
```

#### 2.3 EnumLocaleDescriptionConverter

```xaml
<TextBlock Text="{Binding TestLocaleEnumValue, Converter={x:Static c:EnumLocaleDescriptionConverter.Instance}}" />
```

```c#
class ViewModel
{
    public TestLocaleEnum testLocaleEnumValue { get; set; } = TestLocaleEnum.Second;
}

enum TestLocaleEnum
{
    [LocaleDescription("en", "First", isFallback: true)]
    [LocaleDescription("ja", "ファースト")]
    [LocaleDescription("zh", "第一个")]
    First = 1,

    [LocaleDescription("en", "Second", isFallback: true)]
    [LocaleDescription("ja", "セカンド")]
    [LocaleDescription("zh", "第二个")]
    Second = 2,

    [LocaleDescription("en", "Third", isFallback: true)]
    [LocaleDescription("ja", "サード")]
    [LocaleDescription("zh", "第三个")]
    Third = 3
}
```

#### 2.4 EnumWrapperConverter

EnumWrapperConverter is used to display localized enums. The concept is fairly simple: Enums are annotated with localized string resources and wrapped into EnumWrapper. The view uses the EnumWrapperConverter to extract the localized string resource from the resx file. Following step-by-step instructions show how to localize and bind a simple enum type in a WPF view:

1. Define new public enum type and annotate enum values with [Display] attributes:

```c#
[DataContract] 
public enum PartyMode 
{ 
    [EnumMember] 
    [Display(Name = "PartyMode_Off", ResourceType = typeof(PartyModeResources))] 
    Off, 

    // … 
} 
```

2. Create StringResources.resx and define strings with appropriate keys (e.g. "PartyMode__Off"). Make sure PublicResXFileCodeGenerator is used to generate the .Designer.cs file. (If ResXFileCodeGenerator is used, the resource lookup operations may require more time to complete).
3. Create StringResources.resx for other languages (e.g. StringResources.de.resx) and translate all strings accordingly. Use [Multilingual App Toolkit](https://visualstudiogallery.msdn.microsoft.com/6dab9154-a7e1-46e4-bbfa-18b5e81df520) for easy localization of the defined string resources.
4. Expose enum property in the ViewModel.

```c#
[ObservableProperty]
private PartyMode partyMode;
```

3. Bind to enum property in the View and define Converter={StaticResource EnumWrapperConverter}.

```xaml
<Label Content="{Binding PartyMode, Converter={StaticResource EnumWrapperConverter}}" /> 
```

### 3. Computed Markups

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

Converters can also be created independently of the ComputedConverter binding extensions. This allows an extra level of flexibility. The following is an example of this:

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

This markup extension allows you to create event handlers inline. Aside from allowing void functions, the code is identical to ComputedConverters. However, ComputedEvent exposes a number of variables by default.

```xaml
$sender - The sender of the event.

$eventArgs - The arguments object of the event.

$dataContext - The data context of the sender.

$V0-$V9 - The values set on the ComputedEvent Vx properties.

$P0-$P4 - The values of the ComputedEvent.P0-ComputedEvent.P4 inherited attached properties on sender.

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

### 4. Useful Markups

#### 4.1 DynamicResource

Enable `DynamicResource` to support `Binding`.

```xaml
 <Application.Resources>  
    <c:String
          x:Key="Guid"
          Value="b5ffd5f4-12c1-49ae-bb40-18da2f7643a7" />
</Application.Resources>

<TextBlock Text="{DynamicResource Guid}" />
<TextBlock Text="{c:DynamicResource Guid}" />
<TextBlock Text="{c:DynamicResource {Binding GuidKey}}" />
<TextBlock Text="{c:DynamicResource {Binding GuidKey, Mode=OneTime}}" />
```

```c#
[ObservableObject]
public partial class ViewModel : ReactiveObject
{
    [ObservableProperty]
    private string? guidKey = "Guid";
}
```

#### 4.2 EventBinding

Binding any event from a delegate to an `ICommand`.

Note that there are differences with `ComputedEvent`.

```xaml
<Window xmlns:c="http://schemas.github.com/computedconverters/2024/xaml"
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

#### 4.3 Command

Used when `RelayCommand` is not used.

```xaml
<Element Command={c:Command Execute} />
<Element Command={c:Command ExecuteWithArgumentAsync, CanExecute}
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

#### 4.4 IfExtension

Use the `Conditional expression` in XAML.

```xaml
<Button Command="{c:If {Binding BoolProperty},
                            {Binding OkCommand},
                            {Binding CancelCommand}}" />
```

```xaml
<UserControl>
    <c:If Condition="{Binding IsLoading}">
        <c:If.True>
            <views:LoadingView />
        </c:If.True>
        <c:If.False>
            <views:LoadedView />
        </c:If.False>
    </c:If>
</UserControl>
```

#### 4.5 SwitchExtension

Use the `Switch expression` in XAML.

```xaml
<Image Source="{c:Switch {Binding FileType},
                              {c:Case {x:Static res:FileType.Music}, {StaticResource MusicIcon}},
                              {c:Case {x:Static res:FileType.Video}, {StaticResource VideoIcon}},
                              {c:Case {x:Static res:FileType.Picture}, {StaticResource PictureIcon}},
                              ...
                              {c:Case {StaticResource UnknownFileIcon}}}" />
```

```xaml
<UserControl>
    <c:Switch To="{Binding SelectedViewName}">
        <c:Case Label="View1">
            <views:View1 />
        </c:Case>
        <c:Case Label="{x:Static res:Views.View2}">
            <views:View2 />
        </c:Case>
        <c:Case>
            <views:View404 />
        </c:Case>
    </c:Switch>
</UserControl>
```

#### 4.6 Unbinding

Provide Binding/Markup value for some one not support binding.

```xaml
<TextBlock Text="{DynamicResource {c:Unbinding {Binding GuidKey}}}" />
<!-- Same as -->
<TextBlock Text="{DynamicResource Guid}" />
```

```c#
class ViewModel
{
    private string GuidKey = "Guid";
}
```

#### 4.7 Static

Difference from `x:Static`: supports static singleton.

```xaml
<Label Content="{Binding StaticProperty, Source={c:Static local:StaticClass.Instance}}" />
```

```c#
public class StaticClass
{
    public static StaticClass Instance { get; } = new StaticClass();
    public string StaticProperty { get; set; } = "I'm a StaticProperty";
}
```

#### 4.8 Converter

Use IValueConverter without Binding.

```xaml
<TextBlock
    xmlns:diagnostics="clr-namespace:System.Diagnostics;assembly=System.Runtime"
    Text="Visiable on debugger attached."
    Visibility="{c:Converter Value={x:Static diagnostics:Debugger.IsAttached},
                                   Converter={x:Static c:BoolToVisibilityConverter.Instance}}" />
```

#### 4.9 StringFormat

Use StringFormat without Binding and support `Array<string>`.

```xaml
<ui:TitleBar.Title>
    <c:StringFormat Format="{}{0} v{1}">
        <c:StringFormat.Values>
            <x:Array Type="{c:TypeofString}">
                <x:Array.Items>
                    <c:String Value="ComputedConverters.Test" />
                    <x:Static Member="local:AppConfig.Version" />
                </x:Array.Items>
            </x:Array>
        </c:StringFormat.Values>
    </c:StringFormat>
</ui:TitleBar.Title>
```

#### 4.10 ServiceLocator

Getting IoC support in xaml.

```xaml
<Grid>
    <Grid.Resources>
        <c:SetServiceLocator Value="{c:Unbinding {Binding ServiceProvider, Source={c:Static local:App.Current}}}" />
    </Grid.Resources>
    <c:ServiceLocator Type="{x:Type local:ServiceLocatorTestPage}" />
</Grid>
```

```c#
public partial class App : Application
{
    public ServiceProvider ServiceProvider { get; }
}
```

```c#
public class ServiceLocatorTestPage : UserControl
{
    public ServiceLocatorTestPage()
    {
        AddChild(new TextBlock { Text = "I'm a page named ServiceLocatorTestPage" });
    }
}
```

### 5. ComputedBehaviors

Behaviors that make non bindable property to be bindable property.

```xaml
<Button Height="50" Content="Please mouse over here">
    <i:Interaction.Triggers>
        <i:EventTrigger EventName="MouseEnter">
            <!-- Originally, IsMouseOver is not a bindable property -->
            <b:ButtonSetStateToSourceAction Source="{Binding ButtonMouseOver, Mode=TwoWay}" Property="IsMouseOver" />
        </i:EventTrigger>
        <i:EventTrigger EventName="MouseLeave">
            <!--  Originally, IsMouseOver is a not bindable property  -->
            <b:ButtonSetStateToSourceAction Source="{Binding ButtonMouseOver, Mode=TwoWay}" Property="IsMouseOver" />
        </i:EventTrigger>
    </i:Interaction.Triggers>
</Button>
```

```xaml
<TextBox>
    <i:Interaction.Behaviors>
        <b:TextBoxBindingSupportBehavior
            SelectedText="{Binding SelectedText}"
            SelectionLength="{Binding SelectionLength}"
            SelectionStart="{Binding SelectionStart}" />
    </i:Interaction.Behaviors>
</TextBox>
```

```xaml
<PasswordBox>
    <i:Interaction.Behaviors>
        <b:PasswordBoxBindingSupportBehavior Password="{Binding Password}" />
    </i:Interaction.Behaviors>
</PasswordBox>
```

### 6. ComputedAnimations

```xaml
<Border a:Animations.Primary="{StaticResource FadeIn}" />
```

[More Animations](./ComputedAnimations.WPF/DefaultAnimations.xaml) 

## Examples

[Test](https://github.com/emako/ComputedConverters.NET/tree/master/ComputedConverters.WPF.Test) / [VSEnc](https://github.com/lemutec/VSEnc)

## Thanks

**Idea list here**

https://github.com/OrgEleCho/EleCho.WpfSuite
https://github.com/JohannesMoersch/QuickConverter
https://github.com/thomasgalliker/ValueConverters.NET
https://github.com/CommunityToolkit/Maui
https://github.com/XAMLMarkupExtensions/XAMLMarkupExtensions
https://github.com/DingpingZhang/WpfExtensions
https://github.com/Kinnara/ModernWpf
https://github.com/runceel/Livet
https://github.com/XamlFlair/XamlFlair
