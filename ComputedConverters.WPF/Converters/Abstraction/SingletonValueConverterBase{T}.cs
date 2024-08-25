using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace ComputedConverters;

public abstract class SingletonValueConverterBase<TSelf> : Reactive, IValueConverter where TSelf : SingletonValueConverterBase<TSelf>, new()
{
    private static readonly Lazy<TSelf> _instance = new(() => new TSelf(), LazyThreadSafetyMode.PublicationOnly);

    public static TSelf Instance => _instance.Value;

    public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);
}
