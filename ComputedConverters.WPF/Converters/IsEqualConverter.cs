using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(object), typeof(bool))]
public class IsEqualConverter : EqualityConverter
{
}
