using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ComputedConverters;

public abstract class SmartEnum<TEnum> where TEnum : struct
{
    public static TEnum? FromName(string name, TEnum? @default = default)
    {
        if (Enum.TryParse(name, out TEnum @enum))
        {
            return @enum;
        }
        return @default;
    }

    public static TEnum? FromValue(int? value, TEnum? @default = default)
    {
        if (value != null)
        {
            if (Enum.TryParse(value.ToString(), out TEnum @enum))
            {
                return @enum;
            }
        }
        return @default;
    }

    public static TEnum? FromDescriptionAttribute(string? description, TEnum? @default = default)
    {
        if (description != null)
        {
            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                MemberInfo enumMember = typeof(TEnum).GetMember(enumValue.ToString()!).First();

                if (enumMember.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute attribute && attribute.Description == description)
                {
                    return enumValue;
                }
            }
        }
        return @default;
    }

    public static string? ToDescriptionAttribute(TEnum? value, string? @default = default)
    {
        if (value != null)
        {
            MemberInfo enumMember = typeof(TEnum).GetMember(value.ToString()!).First();

            if (enumMember.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
        }
        return @default;
    }
}
