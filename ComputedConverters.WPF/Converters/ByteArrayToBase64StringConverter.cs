using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(byte[]), typeof(string))]
public sealed class ByteArrayToBase64StringConverter : SingletonValueConverterBase<ByteArrayToBase64StringConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is byte[] byteArray)
        {
            return System.Convert.ToBase64String(byteArray);
        }

        return null;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is string base64String)
        {
            return System.Convert.FromBase64String(base64String);
        }

        return null;
    }
}
