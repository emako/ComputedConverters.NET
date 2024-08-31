using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
}
