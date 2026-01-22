using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(string))]
public sealed class ToStringConverter : SingletonValueConverterBase<ToStringConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && parameter != null)
        {
            MethodInfo? toStringMethod = value.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == nameof(ToString))
                .Where(m => m.GetParameters().Length == 1 && m.GetParameters().First().ParameterType == parameter.GetType())
                .FirstOrDefault();

            if (toStringMethod != null)
            {
                return toStringMethod.Invoke(value, [parameter]);
            }
        }

        return value?.ToString();
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
