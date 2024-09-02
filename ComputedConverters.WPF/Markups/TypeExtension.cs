using System;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Type))]
public sealed class TypeExtension(string typeName) : MarkupExtension
{
    public Type? Type { get; set; } = null;

    private string typeName = typeName;

    public string TypeName
    {
        get => typeName;
        set
        {
            if (typeName != value)
            {
                typeName = value;

                if (Type != null)
                {
                    try
                    {
                        Type = Type.GetType(typeName, false);
                    }
                    catch
                    {
                        Type = null;
                    }
                }
            }
        }
    }

    public TypeExtension() : this(string.Empty)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (!string.IsNullOrWhiteSpace(TypeName))
        {
            if (serviceProvider.GetService(typeof(IXamlTypeResolver)) is IXamlTypeResolver service)
            {
                Type = service.Resolve(TypeName);
            }
            else
            {
                try
                {
                    Type = Type.GetType(TypeName, false);
                }
                catch
                {
                    Type = null;
                }
            }
        }

        return Type;
    }
}
