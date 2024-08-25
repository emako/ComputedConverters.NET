using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringIsNotNullOrEmptyConverter : StringIsNullOrEmptyConverter
{
    public StringIsNotNullOrEmptyConverter()
    {
        IsInverted = true;
    }
}
