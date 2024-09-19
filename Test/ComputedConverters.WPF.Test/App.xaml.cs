using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Windows;

namespace ComputedConverters.Test;

public partial class App : Application
{
    public new static App Current => (Application.Current as App)!;
    public ServiceProvider ServiceProvider { get; }

    public App()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<ServiceLocatorTestPage>();
        ServiceProvider = services.BuildServiceProvider();

        // Setup Quick Converter.
        // Add the System namespace so we can use primitive types (i.e. int, etc.).
        EquationTokenizer.AddNamespace(typeof(object));
        // Add the System.Windows namespace so we can use Visibility.Collapsed, etc.
        EquationTokenizer.AddNamespace(typeof(Visibility));

        //I18nManager.Instance.Add(ComputedConverters.WPF.Test.Properties.Resources.ResourceManager);

        InitializeComponent();

        TestMapper();
    }

    private void TestMapper()
    {
        ReactiveMapperAssemblyResolver.Register();

        {
            TestMapperModel model = new();
            TestMapperViewModel viewModel = new();
            _ = model.MapTo(viewModel);

            Assert.AreEqual(model.Name!.GetHashCode(), viewModel.Name!.GetHashCode());
            Assert.IsFalse(ReferenceEquals(model.Value1, viewModel.Value1));
            Assert.IsTrue(ReferenceEquals(model.Value2, viewModel.Value2));
            Assert.IsTrue(ReferenceEquals(model.Value3, viewModel.Value3));
            Assert.AreEqual(model.ThicknessClass, viewModel.ThicknessClass.ToString());
            Assert.AreEqual(viewModel.ThicknessClass.ToString(), new StringToThicknessClassTypeConverter().Convert(model.ThicknessClass)!.ToString());
        }

        {
            TestMapperModel model = new();
            TestMapperViewModel viewModel = null!;
            viewModel = model.MapTo<TestMapperModel, TestMapperViewModel>();
        }

        {
            TestMapperModel model = new();
            TestMapperViewModel viewModel = new();
            _ = viewModel.MapFrom(model);
        }

        {
            ChannelPlot source = new() { Name = "0x00" };
            ChannelPlot target = new();

            _ = source.MapTo(target);
        }
    }
}

internal sealed class TestMapperModel
{
    public string? Name { get; set; } = "Name";

    public MyClass? Value1 { get; set; } = new() { VV = "VV1" };

    public MyClass? Value2 { get; set; } = new() { VV = "VV2" };

    public MyClass? Value3 { get; set; } = null;

    public int Value4 { get; set; } = default;

    public string ThicknessUnbox { get; set; } = "1,0,1,0";

    public Thickness ThicknessBox { get; set; } = new(1, 0, 1, 0);

    public string ThicknessClass { get; set; } = "1,0,1,0";

    public int? Null { get; set; } = null;
}

internal partial class TestMapperViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name = default!;

    [ObservableProperty]
    [property: ICloneable]
    private MyClass? value1 = default!;

    [ObservableProperty]
    private MyClass? value2 = default!;

    [ObservableProperty]
    private MyClass? value3 = default!;

    [ObservableProperty]
    private int value4 = default;

    [ObservableProperty]
    [property: ITypeConverter(typeof(StringToThicknessTypeConverter))]
    private Thickness thicknessUnbox = default!;

    [ObservableProperty]
    [property: ITypeConverter(typeof(ThicknessToStringTypeConverter))]
    private string thicknessBox = default!;

    [ObservableProperty]
    [property: ITypeConverter(typeof(StringToThicknessClassTypeConverter))]
    private ThicknessClass thicknessClass = default!;

    public int? Null { get; set; } = 96110;
}

public sealed class MyClass : ICloneable
{
    [ICloneable]
    public string? VV { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}

/// <summary>
/// Test unbox
/// </summary>
public sealed class StringToThicknessTypeConverter : ITypeConverter
{
    public override object? Convert(object? value)
    {
        if (value is string str)
        {
            string[] parts = str.Split(',');
            if (parts.Length == 4 &&
                double.TryParse(parts[0].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double left) &&
                double.TryParse(parts[1].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double top) &&
                double.TryParse(parts[2].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double right) &&
                double.TryParse(parts[3].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double bottom))
            {
                return new Thickness(left, top, right, bottom);
            }
        }
        return base.Convert(value);
    }
}

/// <summary>
/// Test box
/// </summary>
public sealed class ThicknessToStringTypeConverter : ITypeConverter
{
    public override object? Convert(object? value)
    {
        if (value is Thickness str)
        {
            return $"{str.Left},{str.Top},{str.Right},{str.Bottom}";
        }
        return base.Convert(value);
    }
}

public sealed class StringToThicknessClassTypeConverter : ITypeConverter
{
    public override object? Convert(object? value)
    {
        if (value is string str)
        {
            string[] parts = str.Split(',');
            if (parts.Length == 4 &&
                double.TryParse(parts[0].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double left) &&
                double.TryParse(parts[1].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double top) &&
                double.TryParse(parts[2].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double right) &&
                double.TryParse(parts[3].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double bottom))
            {
                return new ThicknessClass(left, top, right, bottom);
            }
        }
        return base.Convert(value);
    }
}

public sealed class ThicknessClass(double left, double top, double right, double bottom)
{
    private double left = left;
    private double top = top;
    private double right = right;
    private double bottom = bottom;

    public override string ToString()
    {
        return $"{left},{top},{right},{bottom}";
    }
}

[ReactiveMapperIndicator]
file sealed class ChannelDataMapperIndicator : ReactiveMapperIndicator
{
    public override void CreateMap(IReactiveMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ChannelPlot, ChannelPlot>()
           .ForAllMembersCustom((src, dest) =>
        {
            dest.Name = src.Name;
        });
    }
}

public sealed class ChannelPlot
{
    public string Name { get; set; } = "Name";
}
