using System;

namespace ComputedConverters;

public class RuntimeSingleConvertExceptionEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.RuntimeCodeException;

    public object P { get; private set; } = null!;

    public object V0 { get; private set; } = null!;
    public object V1 { get; private set; } = null!;
    public object V2 { get; private set; } = null!;
    public object V3 { get; private set; } = null!;
    public object V4 { get; private set; } = null!;
    public object V5 { get; private set; } = null!;
    public object V6 { get; private set; } = null!;
    public object V7 { get; private set; } = null!;
    public object V8 { get; private set; } = null!;
    public object V9 { get; private set; } = null!;

    public object Value { get; private set; } = null!;

    public object Parameter { get; private set; } = null!;

    public DynamicSingleConverter Converter { get; private set; } = null!;

    public Exception Exception { get; private set; } = null!;

    public string DebugView { get; private set; } = null!;

    internal RuntimeSingleConvertExceptionEventArgs(string expression, string debugView, object p, object value, object[] values, object parameter, DynamicSingleConverter converter, Exception exception)
        : base(expression)
    {
        DebugView = debugView;
        P = p;
        V0 = values[0];
        V1 = values[1];
        V2 = values[2];
        V3 = values[3];
        V4 = values[4];
        V5 = values[5];
        V6 = values[6];
        V7 = values[7];
        V8 = values[8];
        V9 = values[9];
        Value = value;
        Parameter = parameter;
        Converter = converter;
        Exception = exception;
    }
}
