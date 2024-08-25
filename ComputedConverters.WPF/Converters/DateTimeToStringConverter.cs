using ComputedConverters.Extensions;
using ComputedConverters.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts a <seealso cref="DateTime"/> value to string using formatting specified in <seealso cref="DefaultFormat"/>.
/// </summary>
[ValueConversion(typeof(DateTime), typeof(string))]
public class DateTimeToStringConverter : SingletonValueConverterBase<DateTimeToStringConverter>
{
    protected const string DefaultFormat = "g";
    protected const string DefaultMinValueString = "";

    private readonly ITimeZoneInfo timeZone;

    public DateTimeToStringConverter() : this(SystemTimeZoneInfo.Current)
    {
    }

    internal DateTimeToStringConverter(ITimeZoneInfo timeZone)
    {
        this.timeZone = timeZone;
    }

    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.Register(nameof(Format), typeof(string), typeof(DateTimeToStringConverter), new PropertyMetadata(DefaultFormat));

    public static readonly DependencyProperty MinValueStringProperty =
        DependencyProperty.Register(nameof(MinValueString), typeof(string), typeof(DateTimeToStringConverter), new PropertyMetadata(DefaultMinValueString));

    /// <summary>
    /// The datetime format property.
    /// Standard date and time format strings: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
    /// </summary>
    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    public string MinValueString
    {
        get => (string)GetValue(MinValueStringProperty);
        set => SetValue(MinValueStringProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return MinValueString;
            }

            var localDateTime = dateTime.WithTimeZone(timeZone.Local);
            return localDateTime.ToString(Format, culture);
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            if (value is DateTime dateTime)
            {
                return dateTime;
            }

            if (value is string str)
            {
                if (DateTime.TryParse(str, out var parsedDateTime))
                {
                    return parsedDateTime.WithTimeZone(timeZone.Utc);
                }
            }
        }

        return DependencyProperty.UnsetValue;
    }
}
