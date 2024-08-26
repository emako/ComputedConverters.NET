using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(double), typeof(bool))]
public sealed class DoubleCompareConverter : Reactive, IValueConverter
{
    public static readonly DependencyProperty TargetValueProperty =
        DependencyProperty.Register(nameof(TargetValue), typeof(double), typeof(DoubleCompareConverter), new PropertyMetadata(0d));

    public static readonly DependencyProperty ComparisonProperty =
        DependencyProperty.Register(nameof(Comparison), typeof(NumberComparison), typeof(DoubleCompareConverter), new PropertyMetadata(NumberComparison.Equal));

    public double TargetValue
    {
        get => (double)GetValue(TargetValueProperty);
        set => SetValue(TargetValueProperty, value);
    }

    public NumberComparison Comparison
    {
        get => (NumberComparison)GetValue(ComparisonProperty);
        set => SetValue(ComparisonProperty, value);
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            var current = System.Convert.ToDouble(value);
            var targetValue = TargetValue;

            if (parameter is not null)
            {
                try
                {
                    targetValue = System.Convert.ToDouble(parameter);
                }
                catch
                {
                    ///
                }
            }

            return Comparison switch
            {
                NumberComparison.Equal => current == targetValue,
                NumberComparison.NotEqual => current != targetValue,
                NumberComparison.GreaterThan => current > targetValue,
                NumberComparison.GreaterOrEqual => current >= targetValue,
                NumberComparison.LessThan => current < targetValue,
                NumberComparison.LessOrEqual => current <= targetValue,
                _ => DependencyProperty.UnsetValue
            };
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Number comparison rule
/// </summary>
public enum NumberComparison
{
    /// <summary>
    /// Check if A equals B
    /// </summary>
    Equal,

    /// <summary>
    /// Check if A not equals B
    /// </summary>
    NotEqual,

    /// <summary>
    /// Check if A is greater than B
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Check if A is less than B
    /// </summary>
    LessThan,

    /// <summary>
    /// Check if A is greater than B or equals B
    /// </summary>
    GreaterOrEqual,

    /// <summary>
    /// Check if A is less than B or equals B
    /// </summary>
    LessOrEqual
}
