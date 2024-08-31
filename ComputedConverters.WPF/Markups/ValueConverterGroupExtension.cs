using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(IValueConverter))]
[Localizability(LocalizationCategory.NeverLocalize)]
[ContentProperty(nameof(Converters))]
public sealed class ValueConverterGroupExtension : MarkupExtension, IValueConverter
{
    [ConstructorArgument(nameof(Converters))]
    public ConverterCollection Converters { get; } = [];

    public ValueConverterGroupExtension()
    {
    }

    public ValueConverterGroupExtension(object item0, object item1)
    {
        Converters.Add(item0);
        Converters.Add(item1);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
        Converters.Add(item11);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
        Converters.Add(item11);
        Converters.Add(item12);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
        Converters.Add(item11);
        Converters.Add(item12);
        Converters.Add(item13);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
        Converters.Add(item11);
        Converters.Add(item12);
        Converters.Add(item13);
        Converters.Add(item14);
    }

    public ValueConverterGroupExtension(object item0, object item1, object item2, object item3, object item4, object item5, object item6, object item7, object item8, object item9, object item10, object item11, object item12, object item13, object item14, object item15)
    {
        Converters.Add(item0);
        Converters.Add(item1);
        Converters.Add(item2);
        Converters.Add(item3);
        Converters.Add(item4);
        Converters.Add(item5);
        Converters.Add(item6);
        Converters.Add(item7);
        Converters.Add(item8);
        Converters.Add(item9);
        Converters.Add(item10);
        Converters.Add(item11);
        Converters.Add(item12);
        Converters.Add(item13);
        Converters.Add(item14);
        Converters.Add(item15);
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Converters.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Converters.Aggregate(value, (current, converter) => converter.ConvertBack(current, targetType, parameter, culture));
    }
}

public sealed class ConverterCollection : Collection<IValueConverter>
{
    public void Add(object item)
    {
        if (item is IValueConverter converter)
        {
            base.Add(converter);
        }
        else
        {
            throw new ArgumentException($"[ValueConverterGroupExtension] The type of the parameter must be IValueConverter, " +
                $"but here is {item?.GetType().FullName ?? "null"}");
        }
    }
}
