using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows;

namespace ComputedConverters;

public sealed class EnumWrapperConverter : SingletonValueConverterBase<EnumWrapperConverter>
{
    public static readonly DependencyProperty NameStyleProperty =
        DependencyProperty.Register(nameof(NameStyle), typeof(EnumWrapperConverterNameStyle), typeof(EnumWrapperConverter), new PropertyMetadata(EnumWrapperConverterNameStyle.LongName));

    public EnumWrapperConverterNameStyle NameStyle
    {
        get => (EnumWrapperConverterNameStyle)GetValue(NameStyleProperty)!;
        set => SetValue(NameStyleProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return DependencyProperty.UnsetValue;
        }

        var type = value.GetType();
        var typeInfo = type.GetTypeInfo();
        if (type == targetType ||
            (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(EnumWrapper<>)))
        {
            // If value from source (typically a property in a viewmodel)
            // is already EnumWrapper<T>, no further conversion needs to be done.
            return value;
        }

        if (value is IEnumerable)
        {
            if (typeInfo.IsGenericType)
            {
                var genericType = type.GetGenericArguments()[0];
                var enumWrapperList = typeof(EnumWrapperConverter).GetMethod(nameof(this.CreateMapperList))!
                    .MakeGenericMethod([genericType])
                    .Invoke(this, [value, this.NameStyle]);
                return enumWrapperList;
            }

            throw new ArgumentException("EnumWrapperConverter cannot convert non-generic IEnumerable. Please bind an IEnumerable<T>.");
        }

        object enumWrapper = null!;
        try
        {
            enumWrapper = typeof(EnumWrapperConverter).GetMethod(nameof(this.CreateMapper))!
                .MakeGenericMethod([type])
                .Invoke(this, [value, this.NameStyle])!;
        }
        catch (TargetInvocationException ex)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
        }

        return enumWrapper;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return DependencyProperty.UnsetValue;
        }

        if (targetType == null)
        {
            throw new ArgumentNullException(nameof(targetType), "Argument 'targetType' must not be null");
        }

        if (IsNullable(targetType))
        {
            targetType = Nullable.GetUnderlyingType(targetType)!;
        }

        var type = value.GetType();
        if (type == targetType)
        {
            Debug.WriteLine("EnumWrapperConverter was used to convert between equal types. Consider removing it in this particular situation.");
            return value;
        }

        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(EnumWrapper<>) && type.GetGenericArguments()[0] == targetType)
        {
            // Unpack EnumWrapper<T> if targetType equals T
            object enumValue = null!;
            try
            {
                enumValue = typeof(EnumWrapperConverter).GetMethod(nameof(this.UnpackEnumWrapper))!
                    .MakeGenericMethod([targetType])
                    .Invoke(this, [value])!;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
            }

            return enumValue;
        }

        // TODO GATH: Check if this exception is required
        ////if (value is IEnumerable)
        ////{
        ////    throw new NotSupportedException("EnumWrapperConverter cannot convert back value of type IEnumerable<T>.");
        ////}

        // If value from source (typically a property in a viewmodel)
        // is already EnumWrapper<T>, no further conversion needs to be done.

        object enumWrapper = null!;
        try
        {
            enumWrapper = typeof(EnumWrapperConverter).GetMethod(nameof(this.ConvertMapper))!
                .MakeGenericMethod([targetType])
                .Invoke(this, [value])!;
        }
        catch (TargetInvocationException ex)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
        }

        return enumWrapper;
    }

    public T ConvertMapper<T>(object value)
    {
        return (EnumWrapper<T>)value;
    }

    public EnumWrapper<T> CreateMapper<T>(object value, EnumWrapperConverterNameStyle nameStyle = EnumWrapperConverterNameStyle.LongName)
    {
        return EnumWrapper.CreateWrapper((T)value, nameStyle);
    }

    public T UnpackEnumWrapper<T>(EnumWrapper<T> value)
    {
        return value.Value;
    }

    public IEnumerable<EnumWrapper<T>> CreateMapperList<T>(object values, EnumWrapperConverterNameStyle nameStyle = EnumWrapperConverterNameStyle.LongName)
    {
        foreach (var value in (IEnumerable)values)
        {
            yield return EnumWrapper.CreateWrapper((T)value, nameStyle);
        }
    }

    private static bool IsNullable(Type type)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}

public static class EnumWrapper
{
    /// <summary>
    ///     Creates a list of wrapped values of an enumeration.
    /// </summary>
    /// <typeparam name="TEnumType">Type of the enumeration.</typeparam>
    /// <returns>The wrapped enumeration values.</returns>
    public static IEnumerable<EnumWrapper<TEnumType>> CreateWrappers<TEnumType>()
    {
        var allEnums = Enum.GetValues(typeof(TEnumType)).OfType<TEnumType>();
        return allEnums.Select(x => new EnumWrapper<TEnumType>(x));
    }

    /// <summary>
    ///     Create the wrapped value of an enumeration value.
    /// </summary>
    /// <typeparam name="TEnumType">Type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="nameStyle">The name (short or long) to be considered from the attribute</param>
    /// <returns>The wrapped value.</returns>
    public static EnumWrapper<TEnumType> CreateWrapper<TEnumType>(TEnumType value, EnumWrapperConverterNameStyle nameStyle = EnumWrapperConverterNameStyle.LongName)
    {
        return new EnumWrapper<TEnumType>(value, nameStyle);
    }

    /// <summary>
    ///     Create the wrapped value of an enumeration value.
    /// </summary>
    /// <typeparam name="TEnumType">Type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The wrapped value.</returns>
    public static EnumWrapper<TEnumType> CreateWrapper<TEnumType>(int value)
    {
        return new EnumWrapper<TEnumType>((TEnumType)(object)value);
    }
}

#pragma warning disable CS0660
#pragma warning disable CS0661

[SuppressMessage("", "CS0660:Unable to override Object.Equals since DependencyObject is sealed.", Justification = "<Pending>")]
[SuppressMessage("", "CS0661:Unable to override Object.GetHashCode since DependencyObject is sealed.", Justification = "<Pending>")]
public class EnumWrapper<TEnumType> : Reactive, IEquatable<EnumWrapper<TEnumType>>
{
    private readonly TEnumType value;
    private readonly EnumWrapperConverterNameStyle nameStyle;

    public EnumWrapper(TEnumType value, EnumWrapperConverterNameStyle nameStyle = EnumWrapperConverterNameStyle.LongName)
    {
        this.value = value;
        this.nameStyle = nameStyle;
    }

    public TEnumType Value => value;

    /// <summary>
    /// Use LocalizedValue to bind UI elements to.
    /// To enforce a refresh of LocalizedValue property (e.g. when you change the UI culture at runtime)
    /// just call the <code>Refresh</code> method.
    /// </summary>
    public string LocalizedValue => ToString();

    /// <summary>
    ///     Implicit to string conversion.
    /// </summary>
    /// <returns>Value converted to a localized string.</returns>
    public override string ToString()
    {
        // TODO: Move this code to where the value is set (e.g. ctor)
        var enumType = typeof(TEnumType);
        var fieldInfos = enumType.GetRuntimeFields();

        IEnumerable<FieldInfo> info = fieldInfos.Where(x =>
                                                x.FieldType == enumType &&
                                                x.GetValue(Value!.ToString())!.Equals(Value))
                                                .ToList();
        if (info.Any())
        {
            return (string)info.Select(fieldInfo =>
            {
                var attributes = fieldInfo.GetCustomAttributes(true).ToArray();
                foreach (var attribute in attributes)
                {
                    if (attribute is DisplayAttribute displayAttribute)
                    {
                        if (this.nameStyle == EnumWrapperConverterNameStyle.LongName)
                        {
                            return displayAttribute.GetName();
                        }

                        return displayAttribute.GetShortName();
                    }

                    // HACK: In case the ValueConverters.Forms projects uses a DisplayAttribute from ValueConverters project
                    var type = attribute.GetType();
                    if (type.Name == nameof(DisplayAttribute))
                    {
                        TypeInfo displayAttributeType = null!;
                        try
                        {
                            displayAttributeType = Assembly.Load(new AssemblyName("ComputedConverters")).DefinedTypes.SingleOrDefault(t => t.Name == nameof(DisplayAttribute))!;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }

                        if (displayAttributeType == null)
                        {
                            continue;
                        }

                        if (nameStyle == EnumWrapperConverterNameStyle.LongName)
                        {
                            var getNameMethodInfo = displayAttributeType.GetMethod(nameof(DisplayAttribute.GetName));
                            return getNameMethodInfo!.Invoke(attribute, []);
                        }

                        var getShortNameMethodInfo = displayAttributeType.GetMethod(nameof(DisplayAttribute.GetShortName));
                        return getShortNameMethodInfo!.Invoke(attribute, []);
                    }
                }

                return Value!.ToString();
            }).Single()!;
        }

        return string.Empty;
    }

    /// <summary>
    ///     Checks if some objects are equal.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>True or false.</returns>
    public new bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        var enumWrapper = obj as EnumWrapper<TEnumType>;
        if (enumWrapper! == null!)
        {
            return false;
        }

        return this.Equals(enumWrapper);
    }

    /// <summary>
    ///     Checks if some objects are equal.
    /// </summary>
    /// <param name="other">The other.</param>
    /// <returns>True or false.</returns>
    public bool Equals(EnumWrapper<TEnumType>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Equals(other.Value, this.Value);
    }

    /// <summary>
    ///     Implicit back conversion to the enumeration.
    /// </summary>
    /// <param name="enumToConvert">The enumeration to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator TEnumType(EnumWrapper<TEnumType> enumToConvert)
    {
        return enumToConvert.value;
    }

    /// <summary>
    ///     Implicit back conversion to the enumeration.
    /// </summary>
    /// <param name="enumToConvert">The enumeration to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator int(EnumWrapper<TEnumType> enumToConvert)
    {
        return System.Convert.ToInt32(enumToConvert.value);
    }

    /// <summary>
    ///     Equality comparator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True or false.</returns>
    public static bool operator ==(EnumWrapper<TEnumType> left, EnumWrapper<TEnumType> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Not equal comparator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True or false.</returns>
    public static bool operator !=(EnumWrapper<TEnumType> left, EnumWrapper<TEnumType> right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     The hash code of the object.
    /// </summary>
    /// <returns>The hash code.</returns>
    public new int GetHashCode()
    {
        return Value!.GetHashCode();
    }

    public void Refresh()
    {
        RaisePropertyChanged(nameof(Value));
        RaisePropertyChanged(nameof(LocalizedValue));
    }
}

public enum EnumWrapperConverterNameStyle
{
    LongName = 0,
    ShortName
}

/// <summary>
/// EnumWrapperCollection is an observable collection for enums wrapped in <see cref="EnumWrapper{TEnumType}"/> type.
/// </summary>
/// <typeparam name="TEnumType">Enum type which shall be wrapped.</typeparam>
public class EnumWrapperCollection<TEnumType> : ObservableCollection<EnumWrapper<TEnumType>>
{
    /// <summary>
    /// Creates an instance of the <see cref="EnumWrapperCollection{TEnumType}"/> class
    /// which initializes a collection of <see cref="EnumWrapper{TEnumType}"/>.
    /// </summary>
    public EnumWrapperCollection() : base(EnumWrapper.CreateWrappers<TEnumType>())
    {
    }

    /// <summary>
    /// Creates an instance of the <see cref="EnumWrapperCollection{TEnumType}"/> class.
    /// </summary>
    public EnumWrapperCollection(IEnumerable<EnumWrapper<TEnumType>> enumerable) : base(enumerable)
    {
    }

    public void Refresh()
    {
        foreach (var enumWrapper in this)
        {
            enumWrapper.Refresh();
        }
    }
}
