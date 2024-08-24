using CommunityToolkit.Mvvm.ComponentModel;

namespace ComputedConverters.Test;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string? titleKey = "Key";
}
