using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts non-nullable values to their nullable counterparts and vice versa.
/// </summary>
/// <remarks>
/// Use <c>ConverterParameter</c> to supply a fallback value when the source is <c>null</c> or <see cref="DependencyProperty.UnsetValue"/>.
/// </remarks>
[ValueConversion(typeof(object), typeof(Nullable<>))]
public class NonNullableToNullableConverter : SingletonValueConverterBase<NonNullableToNullableConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Derive the non-nullable type. If none exists, the binding target is not nullable and we simply echo back the source value.
        Type? underlyingType = Nullable.GetUnderlyingType(targetType);

        if (underlyingType == null)
        {
            // Target is not nullable, return the input as-is.
            return value;
        }

        if (value == null || ReferenceEquals(value, DependencyProperty.UnsetValue))
        {
            // When the source offers no value, attempt to use the converter parameter as the default nullable payload.
            object? fallback = TryCoerce(parameter, underlyingType, culture);
            return fallback == null ? null : CreateNullable(underlyingType, fallback);
        }

        // Wrap a real non-nullable value into the requested Nullable<T> by first coercing to the underlying type.
        object? coercedValue = TryCoerce(value, underlyingType, culture);
        return coercedValue == null ? null : CreateNullable(underlyingType, coercedValue);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || ReferenceEquals(value, DependencyProperty.UnsetValue))
        {
            // UI pushed back no value; substitute with converter parameter or the type's default to prevent binding failures.
            object? fallback = TryCoerce(parameter, targetType, culture);
            return fallback ?? Activator.CreateInstance(targetType);
        }

        // Extract the underlying raw value and coerce it (e.g., Nullable<bool> -> bool).
        return TryCoerce(value, targetType, culture);
    }

    /// <summary>
    /// Attempts to convert <paramref name="value"/> into <paramref name="targetType"/> using runtime type checking or <see cref="System.Convert"/>.
    /// </summary>
    /// <returns>The coerced instance or <c>null</c> when conversion is not possible.</returns>
    private static object? TryCoerce(object? value, Type targetType, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }

        return System.Convert.ChangeType(value, targetType, culture);
    }

    /// <summary>
    /// Constructs a boxed <c>Nullable&lt;T&gt;</c> value for the supplied <paramref name="underlyingType"/>.
    /// </summary>
    private static object CreateNullable(Type underlyingType, object value)
    {
        Type nullableType = typeof(Nullable<>).MakeGenericType(underlyingType);
        return Activator.CreateInstance(nullableType, value)!;
    }
}
