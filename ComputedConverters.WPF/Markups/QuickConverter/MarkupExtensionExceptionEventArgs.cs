using System;
using System.Windows.Markup;

namespace ComputedConverters;

public class MarkupExtensionExceptionEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.MarkupException;

    public MarkupExtension MarkupExtension { get; private set; }

    public Exception Exception { get; private set; }

    internal MarkupExtensionExceptionEventArgs(string expression, MarkupExtension markupExtension, Exception exception)
        : base(expression)
    {
        MarkupExtension = markupExtension;
        Exception = exception;
    }
}
