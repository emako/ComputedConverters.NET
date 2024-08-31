using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[ContentProperty(nameof(Cases))]
public class SwitchExtension : MultiBinding
{
    internal const int InvalidIndex = -1;
    internal static readonly object DefaultLabel = new();

    private Binding? _to;
    private int _toIndex = InvalidIndex;

    [ConstructorArgument(nameof(To))]
    public Binding? To
    {
        get => _to;
        set
        {
            if (_toIndex != InvalidIndex)
                throw new InvalidOperationException();

            Bindings.Add(_to = value);
            _toIndex = Bindings.Count - 1;
        }
    }

    public CaseCollection Cases { get; }

    public new IMultiValueConverter Converter
    {
        get => base.Converter;
        private set
        {
            if (base.Converter != null)
            {
                throw new InvalidOperationException($"The {GetType().Name}.Converter property is readonly.");
            }

            base.Converter = value;
        }
    }

    public SwitchExtension()
    {
        Cases = new CaseCollection(this);
        Converter = new MultiValueConverter(this);
    }

    private SwitchExtension(Binding to) : this()
    {
        To = to;
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10, CaseExtension item11) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
        Cases.Add(item11);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10, CaseExtension item11, CaseExtension item12) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
        Cases.Add(item11);
        Cases.Add(item12);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10, CaseExtension item11, CaseExtension item12, CaseExtension item13) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
        Cases.Add(item11);
        Cases.Add(item12);
        Cases.Add(item13);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10, CaseExtension item11, CaseExtension item12, CaseExtension item13, CaseExtension item14) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
        Cases.Add(item11);
        Cases.Add(item12);
        Cases.Add(item13);
        Cases.Add(item14);
    }

    public SwitchExtension(Binding to, CaseExtension item0, CaseExtension item1, CaseExtension item2, CaseExtension item3, CaseExtension item4, CaseExtension item5, CaseExtension item6, CaseExtension item7, CaseExtension item8, CaseExtension item9, CaseExtension item10, CaseExtension item11, CaseExtension item12, CaseExtension item13, CaseExtension item14, CaseExtension item15) : this(to)
    {
        Cases.Add(item0);
        Cases.Add(item1);
        Cases.Add(item2);
        Cases.Add(item3);
        Cases.Add(item4);
        Cases.Add(item5);
        Cases.Add(item6);
        Cases.Add(item7);
        Cases.Add(item8);
        Cases.Add(item9);
        Cases.Add(item10);
        Cases.Add(item11);
        Cases.Add(item12);
        Cases.Add(item13);
        Cases.Add(item14);
        Cases.Add(item15);
    }

    private class MultiValueConverter(SwitchExtension switchExtension) : IMultiValueConverter
    {
        private readonly SwitchExtension _switchExtension = switchExtension;

        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentOption = values[_switchExtension._toIndex];
            if (currentOption == DependencyProperty.UnsetValue) return Binding.DoNothing;

            var @case = _switchExtension.Cases.FirstOrDefault(item => Equals(currentOption, item.Label)) ??
                        _switchExtension.Cases.FirstOrDefault(item => Equals(DefaultLabel, item.Label));

            if (@case == null)
            {
                return null;
            }

            var index = @case.Index;
            return index == InvalidIndex ? @case.Value : values[index];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
