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
}
