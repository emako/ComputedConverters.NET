using System;
using System.Reflection;

namespace ComputedConverters;

/// <summary>
/// For another way to map itself.
/// Provides methods to clone objects, supporting deep, shallow, and ICloneable-based cloning.
/// </summary>
public static class ReactiveMapperCloneSelf
{
    /// <summary>
    /// Performs a deep clone of the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    /// <param name="obj">The object to clone. Can be null.</param>
    /// <returns>A deep clone of the object, or null if the input is null.</returns>
    /// <remarks>
    /// This method creates a new instance of the object and recursively clones all fields.
    /// - **String and primitive types**: Strings and primitive types (e.g., int, double) are not deeply cloned. They are copied as is.
    /// - **Reference types**: Reference types are deeply cloned by recursively cloning their fields.
    /// - **Circular references**: This implementation does not handle circular references, which may cause infinite recursion.
    /// </remarks>
    public static T? DeepCloneSelf<T>(T? obj)
    {
        if (obj == null)
        {
            return default;
        }

        Type type = typeof(T);

        // Handle primitive types and strings
        if (type.IsPrimitive || type == typeof(string))
        {
            return obj;
        }

        // Create a new instance of the same type
        object? newObj = Activator.CreateInstance(type);

        // Clone each field
        foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            object? fieldValue = field.GetValue(obj);

            // Recursively deep clone the field value
            object? clonedFieldValue = DeepClone(fieldValue);

            field.SetValue(newObj, clonedFieldValue);
        }

        return (T?)newObj;

        static object? DeepClone(object? obj)
        {
            if (obj == null)
            {
                return null;
            }

            Type type = obj.GetType();

            // Handle primitive types and strings
            if (type.IsPrimitive || type == typeof(string))
            {
                return obj;
            }

            // Create a new instance of the same type
            object? newObj = Activator.CreateInstance(type);

            // Clone each field
            foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                object? fieldValue = field.GetValue(obj);

                // Recursively deep clone the field value
                object? clonedFieldValue = DeepClone(fieldValue);

                field.SetValue(newObj, clonedFieldValue);
            }

            return newObj;
        }
    }

    /// <summary>
    /// Performs a shallow clone of the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    /// <param name="obj">The object to clone. Can be null.</param>
    /// <returns>A shallow clone of the object, or null if the input is null.</returns>
    /// <remarks>
    /// This method uses the <see cref="Object.MemberwiseClone"/> method to create a shallow copy of the object.
    /// - **Shallow copy**: Only the top-level object is cloned. Nested objects are not deeply cloned, and their references are copied.
    /// - **String and primitive types**: As with deep cloning, strings and primitive types are copied as is.
    /// </remarks>
    public static T? ShallowCloneSelf<T>(T? obj)
    {
        return obj == null ? default : (T?)MemberwiseClone().Invoke(obj, []);

        static MethodBase MemberwiseClone()
        {
            return typeof(object).GetMethod(nameof(MemberwiseClone), BindingFlags.Instance | BindingFlags.NonPublic)!;
        }
    }

    /// <summary>
    /// Clones the given object using the <see cref="ICloneable.Clone"/> method.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone. Must implement <see cref="ICloneable"/>.</typeparam>
    /// <param name="obj">The object to clone. Can be null.</param>
    /// <returns>A clone of the object, or null if the input is null.</returns>
    /// <remarks>
    /// This method relies on the object's implementation of <see cref="ICloneable.Clone"/> for cloning.
    /// - **Custom clone logic**: The cloning behavior depends on the implementation of the <see cref="ICloneable.Clone"/> method in the object's class.
    /// - **Type safety**: Ensure that the object type implements <see cref="ICloneable"/> to avoid runtime errors.
    /// </remarks>
    public static T? CloneSelf<T>(T? obj) where T : ICloneable
    {
        return (T?)obj?.Clone();
    }
}
