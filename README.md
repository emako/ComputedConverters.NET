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


## Projects

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
