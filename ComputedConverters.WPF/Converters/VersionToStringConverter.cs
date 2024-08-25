using System;
using System.Globalization;
using System.Windows;

namespace ComputedConverters;

/// <summary>
/// Converts System.Version objects to string. Parameter can be used to limit the number of Version components to return.
/// [1] Major Version
/// [2] Minor Version
/// [3] Build Number
/// [4] Revision
/// e.g.
///  1. new Version("1.2.3.4").ToString(0) -> ""
///  2. new Version("1.2.3.4").ToString(1) -> "1"
///  3. new Version("1.2.3.4").ToString(2) -> "1.2"
///  4. new Version("1.2.3.4").ToString(3) -> "1.2.3"
///  5. new Version("1.2.3.4").ToString(4) -> "1.2.3.4"
/// </summary>
public sealed class VersionToStringConverter : SingletonValueConverterBase<VersionToStringConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var version = value as Version;
        if (version != null)
        {
            if (int.TryParse(parameter as string, out int fieldCount))
            {
                return version.ToString(fieldCount);
            }

            return version.ToString();
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
