using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ComputedConverters;

public partial class ReactiveMapper
{
    protected internal static readonly LockingConcurrentDictionary<TypePair, Action<object, object>> _methodCache = new();

    internal static LockingConcurrentDictionary<TypePair, Action<object, object>> MethodCache => _methodCache;

    internal static class PropertyCopier
    {
        public static object Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            TypePair methodKey = new(typeof(TSource), typeof(TDestination));

            if (_methodCache.ContainsKey(methodKey))
            {
                var methodValue = _methodCache[methodKey];
                return methodValue.DynamicInvoke(source, destination)!;
            }
            else
            {
                var methodValue = CreateCopyMethod(methodKey.SourceType, methodKey.DestinationType);

                _ = _methodCache.TryAdd(methodKey, new(() => methodValue));
                return methodValue.DynamicInvoke(source, destination)!;
            }
        }

        public static Action<object, object> CreateCopyMethod(Type sourceType, Type targetType)
        {
            DynamicMethod dynamicMethod = new("CopyProperties", null, [typeof(object), typeof(object)], typeof(PropertyCopier).Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sourceProp in properties)
            {
                PropertyInfo? targetProp = targetType.GetProperty(sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);

                if (targetProp != null && targetProp.CanWrite && sourceProp.CanRead && targetProp.PropertyType == sourceProp.PropertyType)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, sourceProp.GetGetMethod()!);
                    il.Emit(OpCodes.Callvirt, targetProp.GetSetMethod()!);
                }
            }

            il.Emit(OpCodes.Ret);

            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }
    }
}
