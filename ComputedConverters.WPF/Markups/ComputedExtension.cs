namespace ComputedConverters;

public sealed class ComputedExtension : QuickBindingExtension
{
    public ComputedExtension() : base()
    {
    }

    public ComputedExtension(string convert) : base(convert)
    {
        Convert = convert;
    }
}
