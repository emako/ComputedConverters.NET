using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Writes Debuge.WriteLine with value, targetType, parameter and culture.
/// </summary>
[ValueConversion(typeof(object), typeof(object))]
public sealed class DebugConverter : SingletonValueConverterBase<DebugConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Debug.WriteLine("DebugConverter.Convert(value={0}, targetType={1}, parameter={2}, culture={3}",
            value ?? "null",
            (object?)targetType ?? "null",
            parameter ?? "null",
            (object?)culture ?? "null");

        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Debug.WriteLine("DebugConverter.ConvertBack(value={0}, targetType={1}, parameter={2}, culture={3}",
             value ?? "null",
             (object?)targetType ?? "null",
             parameter ?? "null",
             (object?)culture ?? "null");

        return value;
    }
}
