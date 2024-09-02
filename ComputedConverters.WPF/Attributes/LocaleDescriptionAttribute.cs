using System;

namespace ComputedConverters;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class LocaleDescriptionAttribute(string locale, string description, bool isFallback = false) : Attribute
{
    private string locale = locale;
    public virtual string Locale => locale;

    private string description = description;
    public virtual string Description => description;

    private bool isFallback = isFallback;
    public virtual bool IsFallback => isFallback;

    public LocaleDescriptionAttribute() : this(string.Empty, string.Empty)
    {
    }

    public LocaleDescriptionAttribute(string description) : this(string.Empty, description)
    {
    }
}
