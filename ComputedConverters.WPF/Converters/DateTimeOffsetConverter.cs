using System;
using System.Windows.Data;

namespace ComputedConverters;

/// <inheritdoc/>
[ValueConversion(typeof(DateTimeOffset), typeof(string))]
public sealed class DateTimeOffsetConverter : DateTimeOffsetToStringConverter
{
}
