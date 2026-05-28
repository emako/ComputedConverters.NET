[![GitHub license](https://img.shields.io/github/license/emako/ComputedConverters.NET)](https://github.com/emako/ComputedConverters.NET/blob/master/LICENSE.txt) [![Actions](https://github.com/emako/ComputedConverters.NET/actions/workflows/library.nuget.yml/badge.svg)](https://github.com/emako/ComputedConverters.NET/actions/workflows/library.nuget.yml)

# ComputedConverters.NET

ComputedConverters provides you with XAML markup that allows you to write inline converters (Vue-like computed method) and expand some converters commonly used.

Salad bowl project here.

## Support framework

Computed series library

| Package/Framework  | WPF                                                          | Avalonia |
| ------------------ | ------------------------------------------------------------ | -------- |
| ComputedConverters | [![NuGet](https://img.shields.io/nuget/v/ComputedConverters.WPF.svg)](https://nuget.org/packages/ComputedConverters.WPF) | TBD      |
| ComputedAnimations | [![NuGet](https://img.shields.io/nuget/v/ComputedAnimations.WPF.svg)](https://nuget.org/packages/ComputedAnimations.WPF) | No Plan  |
| ComputedBehaviors  | [![NuGet](https://img.shields.io/nuget/v/ComputedBehaviors.WPF.svg)](https://nuget.org/packages/ComputedBehaviors.WPF) | No Plan  |

ValueConverters library

| Package/Framework | WPF                                                          | Avalonia                                                     |
| ----------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| ValueConverters   | [![Version](https://img.shields.io/nuget/v/ValueConverters.svg)](https://www.nuget.org/packages/ValueConverters) | [![Version](https://img.shields.io/nuget/v/ValueConverters.Avalonia.svg)](https://www.nuget.org/packages/ValueConverters.Avalonia) |

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

### 1. Reactivity

[Documents/Reactivity.md](Documents/Reactivity.md)


### 2. ValueConverters

[Documents/ValueConverters.md](Documents/ValueConverters.md)

### 3. ComputedMarkups

[Documents/ComputedMarkups.md](Documents/ComputedMarkups.md)

### 4. UsefulMarkups

[Documents/UsefulMarkups.md](Documents/UsefulMarkups.md)

### 5. ComputedBehaviors

[Documents/ComputedBehaviors.md](Documents/ComputedBehaviors.md)

### 6. ComputedAnimations

[Documents/ComputedAnimations.md](Documents/ComputedAnimations.md)

### 7. CalcBinding

`CalcBinding` is a powerful markup extension that lets you write arithmetic, logical, and relational **expressions** directly inside XAML binding paths — no code-behind converter required.

#### Setup

Add the namespace to your XAML:

```xaml
xmlns:c="http://schemas.github.com/computedconverters/2024/xaml"
```

#### Basic Usage

Use `c:Binding` in place of the standard `{Binding}` markup extension. The `Path` property accepts any valid C# expression.

**Arithmetic**

```xaml
<TextBlock Text="{c:Binding Path='Width * 2 + Height'}" />
```

**Boolean expression → Visibility**

```xaml
<!-- Collapsed when IsActive is false (default) -->
<Border Visibility="{c:Binding Path='IsActive'}" />

<!-- Hidden when IsActive is false -->
<Border Visibility="{c:Binding Path='IsActive', FalseToVisibility=Hidden}" />
```

**Logical operators** (`and` / `or` / `not` are accepted as alternatives to `&&` / `||` / `!`):

```xaml
<Button IsEnabled="{c:Binding Path='IsLoggedIn and not IsBusy'}" />
```

**Relational / comparison** (`less` is accepted as `<`):

```xaml
<TextBlock Text="{c:Binding Path='Score less 100 ? \'Low\' : \'High\''}" />
```

**Math functions**

```xaml
<TextBlock Text="{c:Binding Path='Math.Round(Price, 2)'}" />
<Ellipse Width="{c:Binding Path='Math.Sqrt(Area)'}" />
```

**String formatting**

```xaml
<TextBlock Text="{c:Binding Path='FirstName + \" \" + LastName'}" />
```

**Static property / Enum**

```xaml
xmlns:vm="clr-namespace:MyApp.ViewModels"
xmlns:local="clr-namespace:MyApp.Models"

<TextBlock Text="{c:Binding Path='local:AppSettings.Title'}" />
<Border Visibility="{c:Binding Path='Status == local:Status.Active'}" />
```

#### Two-Way Binding

For invertible single-property expressions, two-way binding is supported automatically:

```xaml
<TextBox Text="{c:Binding Path='Price * 1.2', Mode=TwoWay}" />
```

#### Additional Properties

All standard `Binding` properties are supported: `Mode`, `UpdateSourceTrigger`, `ElementName`, `RelativeSource`, `Source`, `StringFormat`, `FallbackValue`, `ValidatesOnDataErrors`, `ValidatesOnExceptions`, etc.

| Property | Default | Description |
|---|---|---|
| `Path` | — | C# expression string |
| `FalseToVisibility` | `Collapsed` | Controls `false → Visibility` mapping (`Collapsed` or `Hidden`) |
| `SingleQuotes` | `false` | Treat `'` as string delimiters instead of `"` |
| `FallbackValue` | `UnsetValue` | Value used when the expression cannot be evaluated |

#### Enabling Trace Logging

To enable diagnostic output, configure the `CalcBindingTracer` trace source in your app config:

```xml
<system.diagnostics>
  <sources>
    <source name="CalcBindingTracer" switchValue="Verbose">
      <listeners>
        <add name="console" type="System.Diagnostics.ConsoleTraceListener" />
      </listeners>
    </source>
  </sources>
</system.diagnostics>
```

#### Replacing the Expression Parser

You can swap the built-in `DynamicExpresso`-based parser with a custom implementation:

```csharp
ComputedConverters.CalcBinding.Binding.ReplaceExpressionParser(new MyCustomParser());
```


## Projects

Examples of projects using this library:

[emako/ComputedConverters.WPF.Test](https://github.com/emako/ComputedConverters.NET/tree/master/ComputedConverters.WPF.Test)

[lemutec/VSEnc](https://github.com/lemutec/VSEnc)

## Thanks

Idea list here, and standing on the shoulders of giants.

[OrgEleCho/EleCho.WpfSuite](OrgEleCho/EleCho.WpfSuite)

[JohannesMoersch/QuickConverter](JohannesMoersch/QuickConverter)

[thomasgalliker/ValueConverters.NET](thomasgalliker/ValueConverters.NET)

[CommunityToolkit/Maui](CommunityToolkit/Maui)

[XAMLMarkupExtensions/XAMLMarkupExtensions](XAMLMarkupExtensions/XAMLMarkupExtensions)

[DingpingZhang/WpfExtensions](DingpingZhang/WpfExtensions)

[Kinnara/ModernWpf](Kinnara/ModernWpf)

[runceel/Livet](runceel/Livet)

[XamlFlair/XamlFlair](XamlFlair/XamlFlair)

[ReactiveUI/Bindings/Converter](https://github.com/reactiveui/ReactiveUI/tree/main/src/ReactiveUI/Bindings/Converter)
