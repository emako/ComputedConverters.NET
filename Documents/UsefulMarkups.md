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
        <c:If.TrueValue>
            <views:LoadingView />
        </c:If.TrueValue>
        <c:If.FalseValue>
            <views:LoadedView />
        </c:If.FalseValue>
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
