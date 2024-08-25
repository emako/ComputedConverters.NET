using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Enum), typeof(object))]
[ValueConversion(typeof(object), typeof(object))]
public sealed class EnumToObjectConverter : SingletonValueConverterBase<EnumToObjectConverter>
{
    public ResourceDictionary Items { get; set; } = null!;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return DependencyProperty.UnsetValue;
        }

        Type type = value.GetType();
        string? key = Enum.GetName(type, value);

        if (Items != null && Items.Contains(key))
        {
            return Items[key];
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
