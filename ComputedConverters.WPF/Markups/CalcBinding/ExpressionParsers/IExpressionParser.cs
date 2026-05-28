using DynamicExpresso;
using System.Collections.Generic;

namespace ComputedConverters.CalcBinding.ExpressionParsers;

public interface IExpressionParser
{
    Lambda Parse(string expressionText, params Parameter[] parameters);

    void SetReference(IEnumerable<ReferenceType> referencedTypes);
}
