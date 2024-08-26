using ComputedConverters;
using System;
using System.Linq;
using System.Linq.Expressions;

internal static class QuickConverterHelpers
{
    private static MethodCallExpression GetFinishedLambda(Tuple<string, Delegate, ParameterExpression[], DataContainer[]> lambda, out ParameterExpression inputP, out ParameterExpression inputV, out ParameterExpression value, out ParameterExpression parameter)
    {
        ParameterExpression val = Expression.Parameter(typeof(object));
        ParameterExpression inPar = Expression.Parameter(typeof(object));
        ParameterExpression inP = Expression.Parameter(typeof(object));
        ParameterExpression inV = Expression.Parameter(typeof(object[]));
        var arguments = lambda.Item3.Select<ParameterExpression, Expression>(par =>
        {
            if (par.Name![0] == 'P')
            {
                return inP;
            }
            else if (par.Name[0] == 'V')
            {
                return Expression.ArrayIndex(inV, Expression.Constant((int)(par.Name[1] - '0')));
            }
            else if (par.Name == "value")
            {
                return val;
            }
            else if (par.Name == "par")
            {
                return inPar;
            }

            throw new Exception("Parameter name error. This shouldn't happen.");
        });
        inputP = inP;
        inputV = inV;
        value = val;
        parameter = inPar;

        return Expression.Call(Expression.Constant(lambda.Item2, lambda.Item2.GetType()), lambda.Item2.GetType().GetMethod("Invoke")!, arguments);
    }
}
