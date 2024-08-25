using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(object))]
public sealed class StringToObjectConverter : SingletonValueConverterBase<StringToObjectConverter>
{
    public ResourceDictionary Items { get; set; } = null!;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string key)
        {
            if (Items != null && Items.Contains(key))
            {
                return Items[key];
            }
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
