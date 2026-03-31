using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Enum), typeof(object))]
[ValueConversion(typeof(string), typeof(object))]
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

        string? key = value as string;

        if (key == null)
        {
            Type type = value.GetType();

            if (!type.IsEnum)
            {
                return DependencyProperty.UnsetValue;
            }

            key = Enum.GetName(type, value);
        }

        if (key != null && TryGetValue(parameter, key, out object? parameterValue))
        {
            return parameterValue;
        }

        if (key != null && Items != null && Items.Contains(key))
        {
            return Items[key];
        }

        return DependencyProperty.UnsetValue;

        static bool TryGetValue(object? parameter, string key, out object? value)
        {
            {
                if (parameter is IDictionary dictionary && dictionary.Contains(key))
                {
                    value = dictionary[key];
                    return true;
                }
            }
            {
                if (parameter is string parameterText)
                {
                    Dictionary<string, string> dictionary = ParseDictionary(parameterText);

                    if (dictionary.TryGetValue(key, out string? dictionaryValue))
                    {
                        value = dictionaryValue;
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }
    }

    public static Dictionary<string, string> ParseDictionary(string parameterText)
    {
        string[] pairs = parameterText.Split(';', ',', '|', '/', '\\');
        Dictionary<string, string> dictionary = [];

        foreach (string pair in pairs)
        {
            if (pair.IndexOf(':') < 0)
            {
                continue;
            }

            string[] keyValue = pair.Split(':');

            if (keyValue.Length < 2)
            {
                continue;
            }

            dictionary[keyValue[0]] = keyValue[1];
        }

        return dictionary;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
