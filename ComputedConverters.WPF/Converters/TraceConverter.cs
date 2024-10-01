using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Writes Trace.WriteLine with value, targetType, parameter and culture.
/// </summary>
[ValueConversion(typeof(object), typeof(object))]
public sealed class TraceConverter : SingletonValueConverterBase<TraceConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Trace.WriteLine($"TraceConverter.Convert(" +
                        $"value={value ?? "null"}, " +
                        $"targetType={(object)targetType ?? "null"}, " +
                        $"parameter={parameter ?? "null"}, " +
                        $"culture={(object)culture ?? "null"})");

        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Trace.WriteLine($"TraceConverter.ConvertBack(" +
                        $"value={value ?? "null"}, " +
                        $"targetType={(object)targetType ?? "null"}, " +
                        $"parameter={parameter ?? "null"}, " +
                        $"culture={(object)culture ?? "null"})");

        return value;
    }
}
