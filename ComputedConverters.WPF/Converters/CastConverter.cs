using System;
using System.Globalization;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(object))]
public sealed class CastConverter : SingletonValueConverterBase<CastConverter>
{
    public Type? TargetType
    {
        get => (Type?)GetValue(TargetTypeProperty);
        set => SetValue(TargetTypeProperty, value);
    }

    public static readonly DependencyProperty TargetTypeProperty =
        DependencyProperty.Register(nameof(TargetType), typeof(Type), typeof(CastConverter), new PropertyMetadata(null));

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            var cast = CreateCastFunction(TargetType ?? targetType);
            return cast.Invoke(value);
        }
        catch
        {
            ///
        }

        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static Func<object?, object?> CreateCastFunction(Type targetType)
    {
        DynamicMethod dynamicMethod = new("Cast", typeof(object), [typeof(object)], true);
        ILGenerator il = dynamicMethod.GetILGenerator();

        il.Emit(OpCodes.Ldarg_0);
        if (targetType.IsValueType)
        {
            il.Emit(OpCodes.Unbox_Any, targetType);
            il.Emit(OpCodes.Box, targetType);
        }
        else
        {
            il.Emit(OpCodes.Castclass, targetType);
        }
        il.Emit(OpCodes.Ret);

        return (Func<object?, object?>)dynamicMethod.CreateDelegate(typeof(Func<object?, object?>));
    }
}
