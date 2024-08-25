using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(string))]
public sealed class StringCaseConverter : SingletonValueConverterBase<StringCaseConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            string stringParameter = $"{parameter}";

            return stringParameter switch
            {
                "U" or "u" => culture.TextInfo.ToUpper(stringValue),
                "L" or "l" => culture.TextInfo.ToLower(stringValue),
                "T" or "t" => culture.TextInfo.ToTitleCase(stringValue),
                _ => throw new ArgumentException($"Parameter '{stringParameter}' is not valid", nameof(parameter)),
            };
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
