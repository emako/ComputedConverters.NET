using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class ObjectMemberDescriptionConverter : SingletonValueConverterBase<ObjectMemberDescriptionConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        DescriptionAttribute? descriptionAttribute = value.GetType()
            .GetField(value.ToString()!)
            ?.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute?.Description ?? value.ToString();
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
