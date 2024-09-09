using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputedConverters.Test;

public partial class App : Application
{
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
    }
}
