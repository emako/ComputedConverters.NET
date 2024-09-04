using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Violeta.Controls;

namespace ComputedConverters.Test;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
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

    [ObservableProperty]
    public double width = 10d;

    [ObservableProperty]
    public double height = 10d;

    public double Area => Computed(() => Width * Height);

    public MainViewModel()
    {
    }

    [RelayCommand]
    private void ChangeWithOrHeight()
    {
        if (Width == 10d)
        {
            Width = 20d;
            Height = 20d;
        }
        else
        {
            Width = 10d;
            Height = 10d;
        }
    }

    [RelayCommand]
    private void Drop(RelayEventArgs param)
    {
        (object _, DragEventArgs e) = param.Deconstruct<DragEventArgs>();

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Toast.Information(string.Join(Environment.NewLine, files));
        }
    }
}

public class StaticClass
{
    public static StaticClass Instance { get; } = new StaticClass();
    public string StaticProperty { get; set; } = "I'm a StaticProperty";
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
