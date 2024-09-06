namespace ComputedConverters;

public sealed class ComputedConverterExtension : QuickConverterExtension
{
    public ComputedConverterExtension() : base()
    {
    }

    public ComputedConverterExtension(string convert) : base(convert)
    {
        Convert = convert;
    }
}
