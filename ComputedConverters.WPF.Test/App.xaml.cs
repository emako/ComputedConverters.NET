using System.Windows;

namespace ComputedConverters.Test;

public partial class App : Application
{
    public App()
    {
        // Setup Quick Converter.
        // Add the System namespace so we can use primitive types (i.e. int, etc.).
        EquationTokenizer.AddNamespace(typeof(object));
        // Add the System.Windows namespace so we can use Visibility.Collapsed, etc.
        EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));

        InitializeComponent();
    }
}
