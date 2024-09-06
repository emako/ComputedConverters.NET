using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

public class QuickValueExtension : MarkupExtension
{
    /// <summary>
    /// The expression to use for calculating the value.
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V0.</summary>
    public object V0 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V1.</summary>
    public object V1 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V2.</summary>
    public object V2 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V3.</summary>
    public object V3 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V4.</summary>
    public object V4 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V5.</summary>
    public object V5 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V6.</summary>
    public object V6 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V7.</summary>
    public object V7 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V8.</summary>
    public object V8 { get; set; } = null!;

    /// <summary>Creates a constant parameter. This can be accessed inside the value expression as $V9.</summary>
    public object V9 { get; set; } = null!;

    /// <summary>
    /// This specifies the context to use for dynamic call sites.
    /// </summary>
    public Type DynamicContext { get; set; } = null!;

    public QuickValueExtension()
    {
    }

    public QuickValueExtension(string valueExpression)
    {
        Value = valueExpression;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        try
        {
            var converter = new QuickConverterExtension(Value)
            {
                V0 = V0,
                V1 = V1,
                V2 = V2,
                V3 = V3,
                V4 = V4,
                V5 = V5,
                V6 = V6,
                V7 = V7,
                V8 = V8,
                V9 = V9,
                DynamicContext = DynamicContext
            };
            var value = (converter.ProvideValue(null!) as IValueConverter)!.Convert(null, typeof(object), null, null);
            if (value is MarkupExtension)
            {
                return (value as MarkupExtension)!.ProvideValue(serviceProvider);
            }

            return value;
        }
        catch (Exception e)
        {
            EquationTokenizer.ThrowQuickConverterEvent(new MarkupExtensionExceptionEventArgs(Value, this, e));
            throw;
        }
    }
}
