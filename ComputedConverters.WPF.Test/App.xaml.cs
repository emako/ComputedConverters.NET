using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
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
        TestMapperModel? model = new();
        TestMapperViewModel? viewModel = new();
        viewModel = model.MapFrom(viewModel);
    }
}

internal class TestMapperModel
{
    public string? Name { get; set; }
}

internal partial class TestMapperViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name;
}

[ReactiveMapperIndicator]
file class ChannelDataMapperIndicator : ReactiveMapperIndicator
{
    public override void CreateMap(IReactiveMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<TestMapperModel, TestMapperViewModel>()
           .ForAllMembersCustom((src, dest) =>
        {
            dest.Name = src.Name;
        });
    }
}
