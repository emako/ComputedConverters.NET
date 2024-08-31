using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class IfExtension : MultiBinding
{
    private const int InvalidIndex = -1;

    private int _conditionIndex = InvalidIndex;
    private int _trueIndex = InvalidIndex;
    private int _falseIndex = InvalidIndex;

    private object? _true = null;
    private object? _false = null;

    [ConstructorArgument(nameof(ConditionValue))]
    public bool ConditionValue { get; set; } = false;

    [ConstructorArgument(nameof(Condition))]
    public BindingBase? Condition
    {
        set => SetProperty(value, ref _conditionIndex, out _);
    }

    [ConstructorArgument(nameof(TrueValue))]
    public object? TrueValue
    {
        set => SetProperty(value, ref _trueIndex, out _true);
    }

    [ConstructorArgument(nameof(FalseValue))]
    public object? FalseValue
    {
        set => SetProperty(value, ref _falseIndex, out _false);
    }

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

    public IfExtension()
    {
        Converter = new MultiValueConverter(this);
    }

    public IfExtension(BindingBase? condition, object? trueValue, object? falseValue) : this()
    {
        Condition = condition;
        TrueValue = trueValue;
        FalseValue = falseValue;
    }

    private void SetProperty<T>(T value, ref int index, out T storage)
    {
        if (index != InvalidIndex)
        {
            throw new InvalidOperationException("Cannot reset the value. ");
        }

        if (value is BindingBase binding)
        {
            Bindings.Add(binding);
            index = Bindings.Count - 1;
            storage = default!;
        }
        else
        {
            storage = value;
        }
    }

    private class MultiValueConverter(IfExtension ifExtension) : IMultiValueConverter
    {
        private readonly IfExtension _ifExtension = ifExtension;

        public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            // Base on ConditionValue Boolean.
            if (values.Length == 0)
            {
                return _ifExtension.ConditionValue
                    ? GetValue(_ifExtension._trueIndex, _ifExtension._true)
                    : GetValue(_ifExtension._falseIndex, _ifExtension._false);
            }

            // Base on Condition Binding.
            var condition = values[_ifExtension._conditionIndex];

            if (condition == DependencyProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            return condition?.CastTo<bool>() ?? false
                ? GetValue(_ifExtension._trueIndex, _ifExtension._true)
                : GetValue(_ifExtension._falseIndex, _ifExtension._false);

            object? GetValue(int index, object? storage)
                => index != InvalidIndex ? values[index] : storage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

file static class ObjectExtensions
{
    public static T? CastTo<T>(this object value)
    {
        return typeof(T).IsValueType && value != null
            ? (T)Convert.ChangeType(value, typeof(T))
            : value is T typeValue ? typeValue : default;
    }
}
