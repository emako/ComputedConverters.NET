using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Enum), typeof(object))]
public sealed class EnumLocaleDescriptionConverter : SingletonValueConverterBase<EnumLocaleDescriptionConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var description = GetEnumLocaleDescription(enumValue, CultureInfo.CurrentUICulture);
            return description;
        }
        return value?.ToString();
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static string GetEnumLocaleDescription(Enum enumValue, CultureInfo? cultureInfo)
    {
        if (enumValue.GetType().GetField(enumValue.ToString()) is FieldInfo { } fi)
        {
            if (fi.GetCustomAttributes(typeof(LocaleDescriptionAttribute), false) is LocaleDescriptionAttribute[] { } attributes)
            {
                if (attributes.Length == 0)
                {
                    return enumValue.ToString();
                }
                else if (attributes.Length == 1)
                {
                    return attributes.First().Description;
                }
                else
                {
                    LocaleDescriptionAttribute? attribute = attributes.FirstOrDefault(a => (cultureInfo ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName.StartsWith(a.Locale, StringComparison.OrdinalIgnoreCase));

                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                    else
                    {
                        LocaleDescriptionAttribute? fallback = attributes.FirstOrDefault(a => a.IsFallback);

                        if (fallback != null)
                        {
                            return fallback.Description;
                        }

                        return attributes.First().Description;
                    }
                }
            }

            return enumValue.ToString();
        }

        return string.Empty;
    }

    public static string GetEnumLocaleDescription(Enum enumValue, string? locale = null)
    {
        return GetEnumLocaleDescription(enumValue, new CultureInfo(
            string.IsNullOrWhiteSpace(locale)
                ? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
                : locale)
            );
    }
}
