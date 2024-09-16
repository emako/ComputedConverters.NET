using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#pragma warning disable CS8765
#pragma warning disable CS8767
#pragma warning disable IDE0290

namespace ComputedConverters;

[DebuggerDisplay("{SourceType.Name}, {DestinationType.Name}")]
public readonly struct TypePair : IEquatable<TypePair>
{
    private class InterfaceComparer : IComparer<Type>
    {
        private readonly List<TypeInfo> _typeInheritance;

        public InterfaceComparer(Type target)
        {
            _typeInheritance = (from type in target.GetTypeInheritance()
                                select type.GetTypeInfo()).Reverse().ToList();
        }

        public int Compare(Type x, Type y)
        {
            bool flag = x.IsAssignableFrom(y);
            bool flag2 = y.IsAssignableFrom(x);
            if (flag && !flag2)
            {
                return -1;
            }

            if (!flag && flag2)
            {
                return 1;
            }

            if (flag && flag2)
            {
                return 0;
            }

            int num = _typeInheritance.FindIndex((TypeInfo type) => type.ImplementedInterfaces.Contains(x));
            int num2 = _typeInheritance.FindIndex((TypeInfo type) => type.ImplementedInterfaces.Contains(y));
            if (num < num2)
            {
                return -1;
            }

            if (num2 > num)
            {
                return 1;
            }

            return 0;
        }
    }

    public Type SourceType { get; }

    public Type DestinationType { get; }

    public bool IsGeneric
    {
        get
        {
            if (!SourceType.IsGenericType)
            {
                return DestinationType.IsGenericType;
            }

            return true;
        }
    }

    public bool IsGenericTypeDefinition
    {
        get
        {
            if (!SourceType.IsGenericTypeDefinition)
            {
                return DestinationType.IsGenericTypeDefinition;
            }

            return true;
        }
    }

    public bool ContainsGenericParameters
    {
        get
        {
            if (!SourceType.ContainsGenericParameters)
            {
                return DestinationType.ContainsGenericParameters;
            }

            return true;
        }
    }

    public static bool operator ==(TypePair left, TypePair right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TypePair left, TypePair right)
    {
        return !left.Equals(right);
    }

    public TypePair(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }

    public bool Equals(TypePair other)
    {
        if (SourceType == other.SourceType)
        {
            return DestinationType == other.DestinationType;
        }

        return false;
    }

    public override bool Equals(object other)
    {
        if (other is TypePair other2)
        {
            return Equals(other2);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCodeCombiner.Combine(SourceType, DestinationType);
    }

    public TypePair? GetOpenGenericTypePair()
    {
        if (!IsGeneric)
        {
            return null;
        }

        Type typeDefinitionIfGeneric = SourceType.GetTypeDefinitionIfGeneric();
        Type typeDefinitionIfGeneric2 = DestinationType.GetTypeDefinitionIfGeneric();
        return new TypePair(typeDefinitionIfGeneric, typeDefinitionIfGeneric2);
    }

    public TypePair CloseGenericTypes(TypePair closedTypes)
    {
        Type[] array = closedTypes.SourceType.GetGenericArguments();
        Type[] array2 = closedTypes.DestinationType.GetGenericArguments();
        if (array.Length == 0)
        {
            array = array2;
        }
        else if (array2.Length == 0)
        {
            array2 = array;
        }

        Type sourceType = (SourceType.IsGenericTypeDefinition ? SourceType.MakeGenericType(array) : SourceType);
        Type destinationType = (DestinationType.IsGenericTypeDefinition ? DestinationType.MakeGenericType(array2) : DestinationType);
        return new TypePair(sourceType, destinationType);
    }

    public IEnumerable<TypePair> GetRelatedTypePairs()
    {
        TypePair @this = this;
        return from destinationType in GetAllTypes(DestinationType)
               from sourceType in GetAllTypes(@this.SourceType)
               select new TypePair(sourceType, destinationType);
    }

    private static IEnumerable<Type> GetAllTypes(Type type)
    {
        IEnumerable<Type> typeInheritance = type.GetTypeInheritance();
        foreach (Type item in typeInheritance)
        {
            yield return item;
        }

        InterfaceComparer comparer = new InterfaceComparer(type);
        IOrderedEnumerable<Type> orderedEnumerable = type.GetTypeInfo().ImplementedInterfaces.OrderByDescending((Type t) => t, comparer);
        foreach (Type item2 in orderedEnumerable)
        {
            yield return item2;
        }
    }
}

file static class TypeExtension
{
    public static Type GetTypeDefinitionIfGeneric(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type;
        }

        return type.GetGenericTypeDefinition();
    }

    public static IEnumerable<Type> GetTypeInheritance(this Type type)
    {
        yield return type;
        Type? baseType = type.BaseType;
        while (baseType != null)
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }
}

file static class HashCodeCombiner
{
    public static int Combine<T1, T2>(T1 obj1, T2 obj2)
    {
        return CombineCodes(obj1!.GetHashCode(), obj2!.GetHashCode());
    }

    public static int CombineCodes(int h1, int h2)
    {
        return ((h1 << 5) + h1) ^ h2;
    }
}
