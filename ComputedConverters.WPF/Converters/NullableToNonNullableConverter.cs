using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts nullable values into non-nullable counterparts and vice versa.
/// </summary>
/// <remarks>
/// Provide a <c>ConverterParameter</c> to control the default when the source value is <c>null</c> or <see cref="DependencyProperty.UnsetValue"/>.
/// </remarks>
[ValueConversion(typeof(Nullable<>), typeof(object))]
public class NullableToNonNullableConverter : SingletonValueConverterBase<NullableToNonNullableConverter>
{
    /// <summary>
    /// Converts a nullable source into a concrete non-nullable value.
    /// </summary>
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Type concreteTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (value == null || ReferenceEquals(value, DependencyProperty.UnsetValue))
        {
            object? fallback = TryCoerce(parameter, concreteTargetType, culture);
            return fallback ?? Activator.CreateInstance(concreteTargetType);
        }

        return TryCoerce(value, concreteTargetType, culture);
    }

    /// <summary>
    /// Wraps a non-nullable value back into the nullable type expected by the binding source.
    /// </summary>
    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(targetType);

        if (underlyingType == null)
        {
            return value;
        }

        if (value == null || ReferenceEquals(value, DependencyProperty.UnsetValue))
        {
            object? fallback = TryCoerce(parameter, underlyingType, culture);
            return fallback == null ? null : CreateNullable(underlyingType, fallback);
        }

        object? coercedValue = TryCoerce(value, underlyingType, culture);
        return coercedValue == null ? null : CreateNullable(underlyingType, coercedValue);
    }

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

    private static object CreateNullable(Type underlyingType, object value)
    {
        Type nullableType = typeof(Nullable<>).MakeGenericType(underlyingType);
        return Activator.CreateInstance(nullableType, value)!;
    }
}
