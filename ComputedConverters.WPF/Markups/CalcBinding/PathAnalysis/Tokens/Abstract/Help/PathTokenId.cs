namespace ComputedConverters.CalcBinding.PathAnalysis;

public class PathTokenId
{
    public PathTokenType PathType { get; private set; }

    public string Value { get; private set; }

    public PathTokenId(PathTokenType pathType, string value)
    {
        PathType = pathType;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj is not PathTokenId o)
            return false;

        return (o.PathType == PathType && o.Value == Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ PathType.GetHashCode();
    }
}
