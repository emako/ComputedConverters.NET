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
    [LocaleDescription("zh", "第一个")]
    First = 1,

    [LocaleDescription("en", "Second", isFallback: true)]
    [LocaleDescription("zh", "第二个")]
    Second = 2,

    [LocaleDescription("en", "Third", isFallback: true)]
    [LocaleDescription("zh", "第三个")]
    Third = 3
}
```

#### 2.4 IsInCollectionConverter

Checks if an element exists in an array, List<T>, HashSet<T>, or other collections. Supports all collection types that implement the IList interface, as well as generic collections that use reflection to call the Contains method. When the parameter is a string, it automatically parses it as a list of values separated by commas, semicolons, or vertical bars.

```xaml
<!-- Using array parameter -->
<TextBlock>
    <TextBlock.Text>
        <Binding Path="SelectedItem">
            <Binding.Converter>
                <c:IsInCollectionConverter />
            </Binding.Converter>
            <Binding.ConverterParameter>
                <x:Array Type="{c:TypeofString}">
                    <sys:String>apple</sys:String>
                    <sys:String>banana</sys:String>
                    <sys:String>orange</sys:String>
                </x:Array>
            </Binding.ConverterParameter>
        </Binding>
    </TextBlock.Text>
</TextBlock>

<!-- Using bound collection -->
<TextBlock Text="{Binding SelectedValue, Converter={x:Static c:IsInCollectionConverter.Instance}, ConverterParameter={Binding AvailableItems}}" />

<!-- Using string parameter (multiple separators supported) -->
<TextBlock Text="{Binding SelectedFruit, Converter={x:Static c:IsInCollectionConverter.Instance}, ConverterParameter='apple, banana; orange | grape'}" />

<!-- Using string parameter with brackets and quotes -->
<TextBlock Text="{Binding SelectedFruit, Converter={x:Static c:IsInCollectionConverter.Instance}, ConverterParameter='[&quot;apple&quot;, &quot;banana&quot;; &quot;orange&quot; | &quot;grape&quot;]'}" />
```

```c#
class ViewModel
{
    public string SelectedItem { get; set; } = "apple";
    public string SelectedFruit { get; set; } = "apple";
    public List<string> AvailableItems { get; } = new() { "apple", "banana", "orange" };
    public HashSet<int> ValidNumbers { get; } = new() { 1, 2, 3, 4, 5 };
}
```

**String Parameter Parsing Rules:**
1. Automatically removes `[` and `]` characters from both ends
2. Uses `,`, `;`, `|` as separators to split the string
3. Trims `"` and spaces from both ends of each item
4. Supports mixing multiple separators

#### 2.5 EnumWrapperConverter

EnumWrapperConverter is used to display localized enums. The concept is fairly simple: Enums are annotated with localized string resources and wrapped into EnumWrapper. The view uses the EnumWrapperConverter to extract the localized string resource from the resx file. Following step-by-step instructions show how to localize and bind a simple enum type in a view:

1. Define new public enum type and annotate enum values with `[Display]` attributes:

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

2. Expose enum property in the ViewModel.

```c#
[ObservableProperty]
private PartyMode partyMode;
```

3. Bind to enum property in the View and define Converter={StaticResource EnumWrapperConverter}.

```xaml
<Label Content="{Binding PartyMode, Converter={StaticResource EnumWrapperConverter}}" /> 
```
