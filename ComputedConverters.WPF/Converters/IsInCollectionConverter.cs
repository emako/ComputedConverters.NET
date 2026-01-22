using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public sealed class IsInCollectionConverter : SingletonValueConverterBase<IsInCollectionConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter == null)
        {
            return false;
        }

        // Handle string parameter - parse as comma/semicolon/pipe separated values
        if (parameter is string stringParameter)
        {
            // Clean up brackets and parse as array
            string cleanedString = stringParameter.Trim('[', ']');
            string[] items = [.. cleanedString.Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim('\'', '"').Trim())
                .Where(item => !string.IsNullOrEmpty(item))
            ];

            return Array.IndexOf(items, value?.ToString()) >= 0;
        }

        // Handle arrays
        if (parameter is Array array)
        {
            return Array.IndexOf(array, value) >= 0;
        }

        // Handle generic IList<T>
        if (parameter is IList list)
        {
            return list.Contains(value);
        }

        // Handle generic ICollection<T> (like HashSet<T>)
        var parameterType = parameter.GetType();
        var containsMethod = parameterType
            .GetMethods()
            .Where(m => m.Name == "Contains")
            .Where(m => m.GetParameters().Length == 1)
            .FirstOrDefault();

        if (containsMethod != null)
        {
            try
            {
                return (bool?)containsMethod.Invoke(parameter, new[] { value }) ?? false;
            }
            catch
            {
                // Fall back to enumeration if Contains method fails
            }
        }

        // Handle generic IEnumerable<T> (fallback for non-standard collections)
        if (parameter is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (Equals(item, value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
