using System.Globalization;

namespace ReactiveMapper;

internal class Program
{
    static void Main(string[] args)
    {
        Clone1(new TestMapperModel1(), new TestMapperViewModel1());
    }

    static void Clone1(TestMapperModel1? source, TestMapperViewModel1? target)
    {
        target!.Thickness = (Thickness)new StringToThicknessTypeConverter().Convert(source!.Thickness)!;
    }

    static void Clone2(TestMapperModel2 source, TestMapperViewModel2 target)
    {
        target.Thickness = source.Thickness;
    }

    static void Clone3(TestMapperModel3? source, TestMapperViewModel3? target)
    {
        target!.Thickness = (string)new ThicknessToStringTypeConverter().Convert(source!.Thickness)!;
    }
}

internal class TestMapperModel1
{
    public string Thickness { get; set; } = "1,0,1,0";
}

internal partial class TestMapperViewModel1
{
    [ITypeConverter(typeof(StringToThicknessTypeConverter))]
    public Thickness Thickness { get; set; }
}

internal class TestMapperModel2
{
    public int Thickness { get; set; } = default;
}

internal partial class TestMapperViewModel2
{
    public int Thickness { get; set; }
}

internal class TestMapperModel3
{
    public Thickness Thickness { get; set; } = new(1, 0, 1, 0);
}

internal partial class TestMapperViewModel3
{
    [ITypeConverter(typeof(ThicknessToStringTypeConverter))]
    public string Thickness { get; set; } = default!;
}

public sealed class StringToThicknessTypeConverter : ITypeConverter
{
    public override object? Convert(object? value)
    {
        if (value is string str)
        {
            string[] parts = str.Split(',');
            if (parts.Length == 4 &&
                double.TryParse(parts[0].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double left) &&
                double.TryParse(parts[1].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double top) &&
                double.TryParse(parts[2].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double right) &&
                double.TryParse(parts[3].Trim(), NumberStyles.Float, Thread.CurrentThread.CurrentUICulture, out double bottom))
            {
                return new Thickness(left, top, right, bottom);
            }
        }
        return base.Convert(value);
    }
}

public sealed class ThicknessToStringTypeConverter : ITypeConverter
{
    public override object? Convert(object? value)
    {
        if (value is Thickness str)
        {
            return $"{str.Left},{str.Top},{str.Right},{str.Bottom}";
        }
        return base.Convert(value);
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ITypeConverterAttribute(string typeName) : Attribute
{
    public string ConverterTypeName { get; } = typeName;

    public ITypeConverterAttribute(Type type) : this(type.AssemblyQualifiedName!)
    {
    }
}

public abstract class ITypeConverter
{
    public virtual object? Convert(object? value)
    {
        return value;
    }
}

internal struct Thickness(double left, double top, double right, double bottom)
{
    public double Left = left;
    public double Top = top;
    public double Right = right;
    public double Bottom = bottom;
}
