using System;
using System.Windows;

namespace ComputedConverters;

public class RuntimeEventHandlerExceptionEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.RuntimeCodeException;

    public object Sender { get; private set; } = null!;

    public object EventArgs { get; private set; } = null!;

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

    public object P0 { get; private set; } = null!;
    public object P1 { get; private set; } = null!;
    public object P2 { get; private set; } = null!;
    public object P3 { get; private set; } = null!;
    public object P4 { get; private set; } = null!;

    public QuickEventHandler Handler { get; private set; } = null!;

    public Exception Exception { get; private set; } = null!;

    public string DebugView { get; private set; } = null!;

    internal RuntimeEventHandlerExceptionEventArgs(object sender, object eventArgs, string expression, string debugView, object[] values, QuickEventHandler handler, Exception exception)
        : base(expression)
    {
        Sender = sender;
        EventArgs = eventArgs;
        DebugView = debugView;
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
        if (sender is DependencyObject)
        {
            P0 = QuickEventExtension.GetP0((sender as DependencyObject)!);
            P1 = QuickEventExtension.GetP1((sender as DependencyObject)!);
            P2 = QuickEventExtension.GetP2((sender as DependencyObject)!);
            P3 = QuickEventExtension.GetP3((sender as DependencyObject)!);
            P4 = QuickEventExtension.GetP4((sender as DependencyObject)!);
        }
        Handler = handler;
        Exception = exception;
    }
}
