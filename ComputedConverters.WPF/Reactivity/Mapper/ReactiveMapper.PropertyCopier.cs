using System;
using System.Reflection;
using System.Reflection.Emit;

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
                var methodValue = CreateCopyMethod(methodKey.SourceType, methodKey.DestinationType);

                _ = _methodCache.TryAdd(methodKey, new(() => methodValue));
                _ = methodValue.DynamicInvoke(source, destination);
                return destination;
            }
        }

        public static Action<object?, object?> CreateCopyMethod(Type sourceType, Type targetType)
        {
            DynamicMethod dynamicMethod = new("CopyProperties", null, [typeof(object), typeof(object)], typeof(PropertyCopier).Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sourceProp in properties)
            {
                PropertyInfo? targetProp = targetType.GetProperty(sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);

                if (targetProp == null)
                {
                    continue;
                }

                if (!sourceProp.CanRead || !targetProp.CanWrite)
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
#if false // Uncheck nullable.
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                    il.Emit(OpCodes.Callvirt, typeof(ICloneable).GetMethod(nameof(ICloneable.Clone))!);
                    il.Emit(OpCodes.Castclass, sourceProp.PropertyType);
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
#else // Check nullable.
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);

                    Label notNullLabel = il.DefineLabel();
                    Label endLabel = il.DefineLabel();

                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Brtrue_S, notNullLabel);

                    il.Emit(OpCodes.Pop);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                    il.Emit(OpCodes.Br_S, endLabel);

                    il.MarkLabel(notNullLabel);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Callvirt, typeof(ICloneable).GetMethod(nameof(ICloneable.Clone))!);
                    il.Emit(OpCodes.Castclass, sourceProp.PropertyType);
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);

                    il.MarkLabel(endLabel);
#endif
                }
                else
                {
                    if (targetProp.PropertyType == sourceProp.PropertyType)
                    {
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                        il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                    }
                }
            }

            il.Emit(OpCodes.Ret);

            return (Action<object?, object?>)dynamicMethod.CreateDelegate(typeof(Action<object?, object?>));
        }
    }
}
