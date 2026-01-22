using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using Wpf.Ui.Violeta.Controls;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

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
    private bool isLoading = true;

    [ObservableProperty]
    private TestEnum testEnumValue = TestEnum.Second;

    [ObservableProperty]
    private TestLocaleEnum testLocaleEnumValue = TestLocaleEnum.Second;

    [ObservableProperty]
    private double width = 10d;

    [ObservableProperty]
    private double height = 10d;

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
    private void ChangeTestValue()
    {
        TestValue = TestValue == "apple" ? "grape" : "apple";
    }

    [RelayCommand]
    private void Drop(RelayEventParameter param)
    {
        (object _, DragEventArgs e) = param.Deconstruct<DragEventArgs>();

        if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] { } files)
            {
                Toast.Information(string.Join(Environment.NewLine, files));
            }
        }
    }

    [ObservableProperty]
    private System.Drawing.Bitmap bitmap = new(ResourceHelper.GetStream("pack://application:,,,/ComputedConverters.Test;component/test.png"));

    [ObservableProperty]
    private TestEnum castConverterObject = TestEnum.First;

    [ObservableProperty]
    private string testValue = "apple";

    public string[] Fruits => new[] { "apple", "banana", "orange" };

    public List<int> Numbers => new() { 1, 2, 3, 4, 5 };

    public HashSet<string> Colors => new() { "red", "green", "blue" };
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

public sealed class ServiceLocatorTestPage : UserControl
{
    public ServiceLocatorTestPage()
    {
        AddChild(new TextBlock { Text = "I'm a page named ServiceLocatorTestPage" });
    }
}

public static class ResourceHelper
{
    static ResourceHelper()
    {
        if (!UriParser.IsKnownScheme("pack"))
            _ = PackUriHelper.UriSchemePack;
    }

    public static byte[] GetBytes(string uriString)
    {
        Uri uri = new(uriString);
        StreamResourceInfo info = Application.GetResourceStream(uri);
        using BinaryReader stream = new(info.Stream);
        return stream.ReadBytes((int)info.Stream.Length);
    }

    public static Stream GetStream(string uriString)
    {
        Uri uri = new(uriString);
        StreamResourceInfo info = Application.GetResourceStream(uri);
        return info?.Stream!;
    }

    public static string GetString(string uriString, Encoding encoding = null!)
    {
        Uri uri = new(uriString);
        StreamResourceInfo info = Application.GetResourceStream(uri);
        using StreamReader stream = new(info.Stream, encoding ?? Encoding.UTF8);
        return stream.ReadToEnd();
    }
}
