using System;

namespace ComputedConverters;

/// <summary>
/// Represents an attribute that allows the author of a value converter to specify
/// the data types involved in the implementation of the converter.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ValueConversionDynamicAttribute : Attribute
{
    private string? _sourceType;

    private string? _targetType;

    /// <summary>
    /// Gets the type this converter converts.
    /// </summary>
    /// <exception cref="Exception" />
    public Type? SourceType
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_sourceType))
            {
                return null;
            }

            try
            {
                return Type.GetType(_sourceType);
            }
            catch
            {
                return null;
            }
        }
        set => _sourceType = value?.AssemblyQualifiedName;
    }

    /// <summary>
    /// Gets the type this converter converts to.
    /// </summary>
    /// <exception cref="Exception" />
    public Type? TargetType
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_targetType))
            {
                return null;
            }

            try
            {
                return Type.GetType(_targetType);
            }
            catch
            {
                return null;
            }
        }
        set => _targetType = value?.AssemblyQualifiedName;
    }

    public ValueConversionDynamicAttribute(string sourceType, string targetType)
    {
        _sourceType = sourceType;
        _targetType = targetType;
    }

    public ValueConversionDynamicAttribute(Type sourceType, Type targetType)
    {
        _sourceType = sourceType.AssemblyQualifiedName;
        _targetType = targetType.AssemblyQualifiedName;
    }

    public ValueConversionDynamicAttribute(Type sourceType, string targetType)
    {
        _sourceType = sourceType.AssemblyQualifiedName;
        _targetType = targetType;
    }

    public ValueConversionDynamicAttribute(string sourceType, Type targetType)
    {
        _sourceType = sourceType;
        _targetType = targetType.AssemblyQualifiedName;
    }
}
