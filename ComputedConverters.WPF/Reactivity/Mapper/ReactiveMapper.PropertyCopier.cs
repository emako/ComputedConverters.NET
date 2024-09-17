using System;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;

namespace ComputedConverters;

public partial class ReactiveMapper
{
    protected internal static readonly LockingConcurrentDictionary<TypePair, Action<object?, object?>> _methodCache = new();

    protected internal static LockingConcurrentDictionary<TypePair, Action<object?, object?>> MethodCache => _methodCache;

    internal class PropertyCopier
    {
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            TypePair methodKey = new(typeof(TSource), typeof(TDestination));

            if (_methodCache.ContainsKey(methodKey))
            {
                var methodValue = _methodCache[methodKey];
                _ = methodValue.DynamicInvoke(source, destination);
                return destination;
            }
            else
            {
                var methodValue = CreatePropertyCloneMethod(methodKey.SourceType, methodKey.DestinationType);

                _ = _methodCache.TryAdd(methodKey, new(() => methodValue));
                _ = methodValue.DynamicInvoke(source, destination);
                return destination;
            }
        }

        public static Action<object?, object?> CreatePropertyCloneMethod(Type sourceType, Type targetType)
        {
            DynamicMethod dynamicMethod = new("PropertyClone", null, [typeof(object), typeof(object)], typeof(PropertyCopier).Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sourceProp in properties)
            {
                PropertyInfo? targetProp = targetType.GetProperty(sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);

                if (targetProp == null || !sourceProp.CanRead || !targetProp.CanWrite)
                {
                    continue;
                }

                if (targetProp.GetCustomAttribute(typeof(NotMappedAttribute)) is NotMappedAttribute { })
                {
                    continue;
                }

                if (targetProp.GetCustomAttribute(typeof(ICloneableAttribute)) is ICloneableAttribute { }
                 && typeof(ICloneable).IsAssignableFrom(sourceProp.PropertyType))
                {
                    // Pseudocode: TAR = ((ICloneable)SRC).Clone();
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                    il.Emit(OpCodes.Callvirt, typeof(ICloneable).GetMethod(nameof(ICloneable.Clone))!);
                    il.Emit(OpCodes.Castclass, sourceProp.PropertyType);
                    if (targetProp.PropertyType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, targetProp.PropertyType);
                    }
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                }
                else if (targetProp.GetCustomAttribute(typeof(ITypeConverterAttribute)) is ITypeConverterAttribute { } typeConverterAttribute)
                {
                    // Pseudocode: TAR = ITypeConverter.Convert(SRC);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Newobj, Type.GetType(typeConverterAttribute.ConverterTypeName)!.GetConstructor(Type.EmptyTypes)!);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                    il.Emit(OpCodes.Callvirt, typeof(ITypeConverter).GetMethod(nameof(ITypeConverter.Convert))!);
                    if (targetProp.PropertyType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, targetProp.PropertyType);
                    }
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                }
                else if (targetProp.PropertyType == sourceProp.PropertyType)
                {
                    // Pseudocode: TAR = SRC;
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                    if (targetProp.PropertyType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, targetProp.PropertyType);
                    }
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                }
            }

            il.Emit(OpCodes.Ret);

            return (Action<object?, object?>)dynamicMethod.CreateDelegate(typeof(Action<object?, object?>));
        }
    }
}
