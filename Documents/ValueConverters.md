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

#### 2.4 EnumWrapperConverter

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
