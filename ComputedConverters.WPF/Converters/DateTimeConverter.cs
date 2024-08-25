using System;
using System.Windows.Data;

namespace ComputedConverters;

/// <inheritdoc/>
[ValueConversion(typeof(DateTime), typeof(string))]
public sealed class DateTimeConverter : DateTimeToStringConverter
{
}
