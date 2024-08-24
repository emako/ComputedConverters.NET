using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

public abstract class SingletonValueConverterBase<TSelf> : IValueConverter where TSelf : SingletonValueConverterBase<TSelf>, new()
{
    private static TSelf? _instance = null;

    public static TSelf Instance => _instance ??= new();

    public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);
}
