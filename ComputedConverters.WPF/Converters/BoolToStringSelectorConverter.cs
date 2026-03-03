using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts a boolean to one of two strings provided via the converter parameter.
/// Parameter format: "falseString;trueString" (semicolon-separated). One-way conversion only.
/// </summary>
[ValueConversion(typeof(bool), typeof(string))]
public sealed class BoolToStringSelectorConverter : SingletonValueConverterBase<BoolToStringSelectorConverter>
{
    /// <inheritdoc />
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b)
            return null;

        if (parameter is string stringParameter)
        {
            string cleanedString = stringParameter.Trim('[', ']');
            string[] items = cleanedString.Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries);

            if (items == null || items.Length < 2)
                return null;
            return b ? items[1] : items[0];
        }

        return null;
    }

    /// <inheritdoc />
    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
