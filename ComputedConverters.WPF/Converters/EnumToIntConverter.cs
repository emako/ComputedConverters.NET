using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts an <see cref="Enum" /> to its underlying <see cref="int" /> value.
/// </summary>
[ValueConversion(typeof(Enum), typeof(int))]
[ValueConversion(typeof(object), typeof(int))]
public sealed class EnumToIntConverter : SingletonValueConverterBase<EnumToIntConverter>
{
    public int DefaultConvertReturnValue { get; set; } = 0;

    public Enum DefaultConvertBackReturnValue { get; set; } = DefaultEnum.Value;

    private enum DefaultEnum { Value }

    /// <summary>
    /// Convert a default <see cref="Enum"/> (i.e., extending <see cref="int"/>) to corresponding underlying <see cref="int"/>
    /// </summary>
    /// <param name="value"><see cref="Enum"/> value to convert</param>
    /// <param name="parameter"></param>
    /// <param name="culture">Unused: Culture to use in the converter</param>
    /// <returns>The underlying <see cref="int"/> value of the passed enum value</returns>
    /// <exception cref="ArgumentException">If value is not an enumeration type</exception>
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        return System.Convert.ToInt32(value);
    }

    /// <summary>
    /// Returns the <see cref="Enum"/> associated with the specified <see cref="int"/> value defined in the targetType
    /// </summary>
    /// <param name="value"><see cref="Enum"/> value to convert</param>
    /// <param name="parameter"></param>
    /// <param name="culture">Unused: Culture to use in the converter</param>
    /// <returns>The underlying <see cref="Enum"/> of the associated targetType</returns>
    /// <exception cref="ArgumentException">If value is not a valid value in the targetType enum</exception>
    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (parameter is not null)
        {
            if (!Enum.IsDefined((Type)parameter, value))
            {
                throw new InvalidEnumArgumentException($"{value} is not valid for {parameter}.");
            }

            return Enum.ToObject((Type)parameter, value);
        }
        else
        {
            return Enum.ToObject(targetType, value);
        }
    }
}
