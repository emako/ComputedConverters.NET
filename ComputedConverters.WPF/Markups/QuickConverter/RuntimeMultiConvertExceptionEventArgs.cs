using System;

namespace ComputedConverters;

public class RuntimeMultiConvertExceptionEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.RuntimeCodeException;

    public object P0 { get; private set; } = null!;
    public object P1 { get; private set; } = null!;
    public object P2 { get; private set; } = null!;
    public object P3 { get; private set; } = null!;
    public object P4 { get; private set; } = null!;
    public object P5 { get; private set; } = null!;
    public object P6 { get; private set; } = null!;
    public object P7 { get; private set; } = null!;
    public object P8 { get; private set; } = null!;
    public object P9 { get; private set; } = null!;

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

    public DynamicMultiConverter Converter { get; private set; } = null!;

    public Exception Exception { get; private set; } = null!;

    public string DebugView { get; private set; } = null!;

    internal RuntimeMultiConvertExceptionEventArgs(string expression, string debugView, object[] p, int[] pIndices, object value, object[] values, object parameter, DynamicMultiConverter converter, Exception exception)
        : base(expression)
    {
        DebugView = debugView;
        if (p != null)
        {
            for (int i = 0; i < p.Length; ++i)
            {
                switch (pIndices[i])
                {
                    case 0: P0 = p[i]; break;
                    case 1: P1 = p[i]; break;
                    case 2: P2 = p[i]; break;
                    case 3: P3 = p[i]; break;
                    case 4: P4 = p[i]; break;
                    case 5: P5 = p[i]; break;
                    case 6: P6 = p[i]; break;
                    case 7: P7 = p[i]; break;
                    case 8: P8 = p[i]; break;
                    case 9: P9 = p[i]; break;
                }
            }
        }
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
