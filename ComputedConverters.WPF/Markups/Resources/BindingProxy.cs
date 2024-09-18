using System.Windows;

namespace ComputedConverters;

/// <summary>
/// Proxy for binding
/// </summary>
/// <remarks>
/// You can bind data to the Data of the BindingProxy and make the BindingProxy a static resource, <br/>
/// so that elements outside of the main visual tree can also bind to the corresponding data through the BindingProxy
/// </remarks>
public class BindingProxy : Freezable
{
    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));

    protected override Freezable CreateInstanceCore() => new BindingProxy();
}
