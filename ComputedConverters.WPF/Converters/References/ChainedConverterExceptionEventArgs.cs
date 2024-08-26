﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputedConverters;

public class ChainedConverterExceptionEventArgs : QuickConverterEventArgs
{
    public override QuickConverterEventType Type => QuickConverterEventType.ChainedConverterException;

    public IValueConverter ChainedConverter { get; private set; }

    public object InputValue { get; private set; }

    public Type TargetType { get; private set; }

    public object Parameter { get; private set; }

    public CultureInfo Culture { get; private set; }

    public bool ConvertingBack { get; private set; }

    public object ParentConverter { get; private set; }

    public Exception Exception { get; private set; }

    internal ChainedConverterExceptionEventArgs(string expression, object inputValue, Type targetType, object parameter, CultureInfo culture, bool convertingBack, IValueConverter chainedConverter, object parentConverter, Exception exception)
        : base(expression)
    {
        InputValue = inputValue;
        TargetType = targetType;
        Parameter = parameter;
        Culture = culture;
        ConvertingBack = convertingBack;
        ChainedConverter = chainedConverter;
        ParentConverter = parentConverter;
        Exception = exception;
    }
}
