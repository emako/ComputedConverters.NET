using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class DebugConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Debug.WriteLine("DebugConverter.Convert(value={0}, targetType={1}, parameter={2}, culture={3}",
            value ?? "null",
            (object?)targetType ?? "null",
            parameter ?? "null",
            (object?)culture ?? "null");

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Debug.WriteLine("DebugConverter.ConvertBack(value={0}, targetType={1}, parameter={2}, culture={3}",
             value ?? "null",
             (object?)targetType ?? "null",
             parameter ?? "null",
             (object?)culture ?? "null");

        return value;
    }
}
