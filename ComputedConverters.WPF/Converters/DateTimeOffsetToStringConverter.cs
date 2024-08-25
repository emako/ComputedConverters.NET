using ComputedConverters.Extensions;
using ComputedConverters.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputedConverters;

/// <summary>
/// Converts a <seealso cref="DateTimeOffset"/> value to string using formatting specified in <seealso cref="DefaultFormat"/>.
/// </summary>
[ValueConversion(typeof(DateTimeOffset), typeof(string))]
public class DateTimeOffsetToStringConverter : SingletonValueConverterBase<DateTimeOffsetToStringConverter>
{
    protected const string DefaultFormat = "g";
    protected const string DefaultMinValueString = "";

    private readonly ITimeZoneInfo timeZone;

    public DateTimeOffsetToStringConverter() : this(SystemTimeZoneInfo.Current)
    {
    }

    internal DateTimeOffsetToStringConverter(ITimeZoneInfo timeZone)
    {
        this.timeZone = timeZone;
    }

    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.Register(nameof(Format), typeof(string), typeof(DateTimeOffsetToStringConverter), new PropertyMetadata(DefaultFormat));

    public static readonly DependencyProperty MinValueStringProperty =
        DependencyProperty.Register(nameof(MinValueString), typeof(string), typeof(DateTimeOffsetToStringConverter), new PropertyMetadata(DefaultMinValueString));

    /// <summary>
    /// The datetime format property.
    /// Standard date and time format strings: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
    /// </summary>
    public string Format
    {
        get => (string)this.GetValue(FormatProperty);
        set => this.SetValue(FormatProperty, value);
    }

    public string MinValueString
    {
        get => (string)this.GetValue(MinValueStringProperty);
        set => this.SetValue(MinValueStringProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset == DateTimeOffset.MinValue)
            {
                return this.MinValueString;
            }

            return dateTimeOffset.WithTimeZone(this.timeZone.Local).ToString(this.Format, culture);
        }

        return DependencyProperty.UnsetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset;
            }

            if (value is string str)
            {
                if (DateTimeOffset.TryParse(str, out var parsedDateTimeOffset))
                {
                    return parsedDateTimeOffset.WithTimeZone(this.timeZone.Utc);
                }
            }
        }

        return DependencyProperty.UnsetValue;
    }
}
