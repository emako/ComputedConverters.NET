using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(ReactiveObject), typeof(object))]
public sealed class ComputedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ReactiveObject react)
        {
            if (parameter is string expression)
            {
                react.Computed(CreateExpression(expression));
            }
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static Expression<Func<object>> CreateExpression(string expressionString)
    {
        var scriptOptions = ScriptOptions.Default
            .WithReferences(AppDomain.CurrentDomain.GetAssemblies())
            .WithImports("System");

        var script = CSharpScript.Create<object>($"Func<object> expr = () => {expressionString}; return expr();", scriptOptions);
        var compiledScript = script.Compile();

        var result = script.RunAsync().Result.ReturnValue as Expression<Func<object>>;

        return result;
    }
}
