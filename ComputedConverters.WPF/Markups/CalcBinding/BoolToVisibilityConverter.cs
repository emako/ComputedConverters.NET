using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters.CalcBinding;

/// <summary>
/// Common BoolToVisibility converter with FalseToVisibility parameter
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    public BoolToVisibilityConverter()
    {
        FalseToVisibility = FalseToVisibility.Collapsed;
    }

    public BoolToVisibilityConverter(FalseToVisibility falseToVisibility)
    {
        FalseToVisibility = falseToVisibility;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = value is bool b ? b : (bool)(dynamic)value;

        if (boolValue)
            return Visibility.Visible;

        return (FalseToVisibility == FalseToVisibility.Collapsed) ? Visibility.Collapsed : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (Visibility)value == Visibility.Visible;
    }

    public FalseToVisibility FalseToVisibility { get; set; }
}
