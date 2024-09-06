using System;
using System.Linq;
using System.Reflection;

namespace ComputedConverters;

/// <summary>
/// Source: Mono and .Net Reference Source
/// https://searchcode.com/codesearch/view/7229840/
/// http://referencesource.microsoft.com/#System.ComponentModel.DataAnnotations/DataAnnotations/DisplayAttribute.cs
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Method, AllowMultiple = false)]
public class DisplayAttribute : Attribute
{
    private const string PropertyNotSetMessage = "The {0} property has not been set.  Use the Get{0} method to get the value.";

    private bool? autoGenerateField;
    private bool? autoGenerateFilter;
    private int? order;

    public Type ResourceType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string Prompt { get; set; } = null!;

    public bool AutoGenerateField
    {
        get
        {
            if (!autoGenerateField.HasValue)
            {
                throw new InvalidOperationException(string.Format(PropertyNotSetMessage, nameof(AutoGenerateField)));
            }

            return autoGenerateField.Value;
        }
        set
        {
            autoGenerateField = value;
        }
    }

    public bool AutoGenerateFilter
    {
        get
        {
            if (autoGenerateFilter == null)
            {
                throw new InvalidOperationException(string.Format(PropertyNotSetMessage, nameof(AutoGenerateFilter)));
            }

            return autoGenerateFilter.Value;
        }
        set
        {
            autoGenerateFilter = value;
        }
    }

    public int Order
    {
        get
        {
            if (order == null)
            {
                throw new InvalidOperationException(string.Format(PropertyNotSetMessage, nameof(Order)));
            }

            return order.Value;
        }
        set
        {
            order = value;
        }
    }

    private string? GetLocalizedString(string propertyName, string key)
    {
        // If we don't have a resource or a key, go ahead and fall back on the key
        if (ResourceType == null || key == null)
        {
            return key;
        }

        var property = ResourceType.GetRuntimeProperty(key);
        if (property == null)
        {
            // In case the .resx is generated with ResXFileCodeGenerator instead of PublicResXFileCodeGenerator.
            property = ResourceType.GetTypeInfo().DeclaredProperties.FirstOrDefault(x => x.Name == key);
        }

        // Strings are only valid if they are public static strings
        var isValid = false;
        if (property != null && property.PropertyType == typeof(string))
        {
            var getter = property.GetMethod;

            // Gotta have a public static getter on the property
            if (getter != null && getter.IsStatic)
            {
                isValid = true;
            }
        }

        // If it's not valid, go ahead and throw an InvalidOperationException
        if (!isValid)
        {
            var message =
                $"Cannot retrieve property '{propertyName}' because localization failed. " +
                $"Type '{ResourceType} is not public or does not contain a public static string property with the name '{key}'.";
            throw new InvalidOperationException(message);
        }

        return (string?)property?.GetValue(null, null);
    }

    public bool? GetAutoGenerateField()
    {
        return autoGenerateField;
    }

    public bool? GetAutoGenerateFilter()
    {
        return autoGenerateFilter;
    }

    public int? GetOrder()
    {
        return order;
    }

    public string? GetName()
    {
        return GetLocalizedString(nameof(Name), Name);
    }

    public string? GetShortName()
    {
        // Short name falls back on Name if the short name isn't set
        return GetLocalizedString(nameof(ShortName), ShortName) ?? GetName();
    }

    public string? GetDescription()
    {
        return GetLocalizedString(nameof(Description), Description);
    }

    public string? GetPrompt()
    {
        return GetLocalizedString(nameof(Prompt), Prompt);
    }

    public string? GetGroupName()
    {
        return GetLocalizedString(nameof(GroupName), GroupName);
    }
}
