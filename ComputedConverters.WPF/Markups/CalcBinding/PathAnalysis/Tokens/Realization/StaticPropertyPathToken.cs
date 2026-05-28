using System;
using System.Collections.Generic;

namespace ComputedConverters.CalcBinding.PathAnalysis;

public class StaticPropertyPathToken : PropertyPathToken
{
    public string Class { get; private set; }
    public string Namespace { get; private set; }

    private readonly PathTokenId id;
    public override PathTokenId Id => id;

    public StaticPropertyPathToken(int start, int end, string @namespace, string @class, IEnumerable<string> properties)
        : base(start, end, properties)
    {
        Class = @class;
        Namespace = @namespace;
        id = new PathTokenId(PathTokenType.StaticProperty, string.Format("{0}:{1}.{2}", Namespace, Class, string.Join(".", Properties)));
    }
}
