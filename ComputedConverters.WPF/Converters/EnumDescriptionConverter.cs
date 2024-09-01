using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(Enum), typeof(object))]
public sealed class EnumDescriptionConverter : SingletonValueConverterBase<EnumDescriptionConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var description = GetEnumDescription(enumValue);
            return description;
        }
        return value?.ToString();
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string GetEnumDescription(Enum enumValue)
    {
        if (enumValue.GetType().GetField(enumValue.ToString()) is FieldInfo { } fi)
        {
            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] { } attributes)
            {
                return (attributes.Length > 0) ? attributes[0].Description : enumValue.ToString();
            }
        }

        return enumValue.ToString();
    }
}
