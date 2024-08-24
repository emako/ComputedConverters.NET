using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class ConverterExtension() : MarkupExtension
{
    public object? Value { get; set; }
    public IValueConverter? Converter { get; set; }
    public object? Parameter { get; set; }
    public CultureInfo? Culture { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Converter == null)
        {
            return Value;
        }
        else
        {
            Type targetType = typeof(object);

            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
            {
                var targetProperty = provideValueTarget.TargetProperty;

                if (targetProperty is DependencyProperty dp)
                {
                    targetType = dp.PropertyType;
                }
                else if (targetProperty is PropertyInfo prop)
                {
                    targetType = prop.PropertyType;
                }
            }

            return Converter.Convert(Value, targetType, Parameter, Culture);
        }
    }
}
