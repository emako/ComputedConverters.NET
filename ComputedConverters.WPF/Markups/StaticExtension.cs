using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Type))]
[ContentProperty(nameof(Member))]
[DefaultProperty(nameof(Member))]
public sealed class StaticExtension(string member) : MarkupExtension
{
    public Type MemberType { get; set; } = null!;

    public string Member { get; set; } = member;

    public StaticExtension() : this(string.Empty)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(Member))
        {
            throw new InvalidOperationException("The member property must be set!");
        }

        string memberName = Member;
        int memberTypeEndsAt = memberName.LastIndexOf('.');
        if (memberTypeEndsAt != -1)
        {
            string typeName = memberName.Substring(0, memberTypeEndsAt);

            if (serviceProvider.GetService(typeof(IXamlTypeResolver)) is IXamlTypeResolver service)
            {
                MemberType = service.Resolve(typeName);
            }
            else
            {
                try
                {
                    MemberType = Type.GetType(typeName, false)!;
                }
                catch
                {
                    return null;
                }
            }

            memberName = memberName.Substring(memberTypeEndsAt + 1);
        }

        return GetValueFromMember(MemberType, memberName);
    }

    private object? GetValueFromMember(Type getMemberType, string getMemberName)
    {
        if (string.IsNullOrWhiteSpace(getMemberName))
        {
            return null;
        }

        if (getMemberType.IsEnum)
        {
            return Enum.Parse(getMemberType, getMemberName, true);
        }

        // Avoid the property use `new` keyword.
        if (getMemberType
            .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(prop => prop.Name == getMemberName)
            .FirstOrDefault() is PropertyInfo pi)
        {
            if (!pi.CanRead)
            {
                throw new InvalidOperationException("No static get accessor for property " + getMemberName + ".");
            }

            return pi.GetValue(null, null);
        }

        // Avoid the field use `new` keyword.
        if (getMemberType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(field => field.Name == Member)
            .FirstOrDefault() is FieldInfo fi)
        {
            return fi.GetValue(null);
        }

        throw new InvalidOperationException("No static enum, property or field " + getMemberName + " available in " + getMemberType.FullName);
    }
}
