using System;

namespace ComputedConverters;

/// <summary>
/// <seealso cref="System.ComponentModel.TypeConverterAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ITypeConverterAttribute(string typeName) : Attribute
{
    public string ConverterTypeName { get; } = typeName;

    public ITypeConverterAttribute(Type type) : this(type.AssemblyQualifiedName!)
    {
    }
}

/// <summary>
/// <seealso cref="System.ComponentModel.TypeConverter"/>
/// </summary>
public abstract class ITypeConverter
{
    public virtual object? Convert(object? value)
    {
        return value;
    }
}
