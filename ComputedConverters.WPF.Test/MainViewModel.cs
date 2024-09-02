using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;

namespace ComputedConverters.Test;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string? guidKey = "Guid";

    [RelayCommand]
    private void ChangeGuid()
    {
        Application.Current.Resources[GuidKey] = Guid.NewGuid().ToString();
    }

    [ObservableProperty]
    private int state = 1;

    [ObservableProperty]
    private int value = 2;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public TestEnum testEnumValue = TestEnum.Second;

    [ObservableProperty]
    public TestLocaleEnum testLocaleEnumValue = TestLocaleEnum.Second;

    public MainViewModel()
    {
    }
}

public enum TestEnum
{
    [Description("First")]
    First = 1,

    [Description("Second")]
    Second = 2,

    [Description("Third")]
    Third = 3
}

public enum TestLocaleEnum
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
