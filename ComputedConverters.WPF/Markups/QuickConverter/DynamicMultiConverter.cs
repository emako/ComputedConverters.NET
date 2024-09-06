﻿using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using Expression = System.Linq.Expressions.Expression;

namespace ComputedConverters;

public class DynamicMultiConverter : IMultiValueConverter
{
    private static Type _namedObjectType = typeof(DependencyObject).Assembly.GetType("MS.Internal.NamedObject", false)!;

    private static ConcurrentDictionary<Type, Func<object, object>> castFunctions = new();

    public string ConvertExpression { get; private set; } = null!;
    public string[] ConvertBackExpression { get; private set; } = null!;
    public Exception LastException { get; private set; } = null!;
    public int ExceptionCount { get; private set; }

    public string ConvertExpressionDebugView { get; private set; } = null!;
    public string[] ConvertBackExpressionDebugView { get; private set; } = null!;

    public Type[] PTypes { get; private set; } = null!;

    public Type ValueType { get; private set; } = null!;

    public object[] Values => _values;

    private int[] _pIndices = null!;

    private Func<object[], object[], object> _converter;
    private Func<object, object[], object>[] _convertBack;
    private object[] _values = null!;
    private DataContainer[] _toDataContainers = null!;
    private DataContainer[] _fromDataContainers = null!;
    private IValueConverter _chainedConverter = null!;

    public DynamicMultiConverter(Func<object[], object[], object> converter, Func<object, object[], object>[] convertBack, object[] values, string convertExp, string convertExpDebug, string[] convertBackExp, string[] convertBackExpDebug, Type[] pTypes, int[] pIndices, Type vType, DataContainer[] toDataContainers, DataContainer[] fromDataContainers, IValueConverter chainedConverter)
    {
        _converter = converter;
        _convertBack = convertBack;
        _values = values;
        _toDataContainers = toDataContainers;
        _fromDataContainers = fromDataContainers;
        ConvertExpression = convertExp;
        ConvertBackExpression = convertBackExp;
        PTypes = pTypes;
        _pIndices = pIndices;
        ValueType = vType;
        _chainedConverter = chainedConverter;
    }

    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        for (int i = 0; i < values.Length; ++i)
        {
            if (values[i] == DependencyProperty.UnsetValue || _namedObjectType.IsInstanceOfType(values[i]) || (PTypes[i] != null && !PTypes[i].IsInstanceOfType(values[i])))
            {
                return DependencyProperty.UnsetValue;
            }
        }

        object result;
        try
        {
            result = _converter(values, _values);
        }
        catch (Exception e)
        {
            LastException = e;
            ++ExceptionCount;
            if (Debugger.IsAttached)
            {
                Console.WriteLine("QuickMultiConverter Exception (\"" + ConvertExpression + "\") - " + e.Message + (e.InnerException != null ? " (Inner - " + e.InnerException.Message + ")" : ""));
            }

            EquationTokenizer.ThrowQuickConverterEvent(new RuntimeMultiConvertExceptionEventArgs(ConvertExpression, ConvertExpressionDebugView, values, _pIndices, null!, _values, parameter, this, e));
            return DependencyProperty.UnsetValue;
        }
        finally
        {
            if (_toDataContainers != null)
            {
                foreach (DataContainer container in _toDataContainers)
                {
                    container.Value = null;
                }
            }
        }

        if (result == DependencyProperty.UnsetValue || result == Binding.DoNothing)
        {
            return result;
        }

        if (_chainedConverter != null)
        {
            try
            {
                result = _chainedConverter.Convert(result, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                EquationTokenizer.ThrowQuickConverterEvent(new ChainedConverterExceptionEventArgs(ConvertExpression, result, targetType, parameter, culture, false, _chainedConverter, this, e));
                return DependencyProperty.UnsetValue;
            }
        }

        result = CastResult(result, targetType);
        return result;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        if (_chainedConverter != null)
        {
            try
            {
                value = _chainedConverter.ConvertBack(value, typeof(object), parameter, culture);
            }
            catch (Exception e)
            {
                EquationTokenizer.ThrowQuickConverterEvent(new ChainedConverterExceptionEventArgs(ConvertExpression, value, typeof(object), parameter, culture, true, _chainedConverter, this, e));
                return new object[targetTypes.Length].Select(o => value).ToArray();
            }

            if (value == DependencyProperty.UnsetValue || value == Binding.DoNothing)
            {
                return new object[targetTypes.Length].Select(o => value).ToArray();
            }
        }

        object[] ret = new object[_convertBack.Length];

        if (ValueType != null && !ValueType.IsInstanceOfType(value))
        {
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = Binding.DoNothing;
            }

            return ret;
        }

        for (int i = 0; i < _convertBack.Length; ++i)
        {
            try
            {
                ret[i] = _convertBack[i](value, _values);
            }
            catch (Exception e)
            {
                LastException = e;
                ++ExceptionCount;
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("QuickMultiConverter Exception (\"" + ConvertBackExpression[i] + "\") - " + e.Message + (e.InnerException != null ? " (Inner - " + e.InnerException.Message + ")" : ""));
                }

                EquationTokenizer.ThrowQuickConverterEvent(new RuntimeMultiConvertExceptionEventArgs(ConvertBackExpression[i], ConvertBackExpressionDebugView[i], null!, _pIndices, value, _values, parameter, this, e));
                ret[i] = DependencyProperty.UnsetValue;
            }
            ret[i] = CastResult(ret[i], targetTypes[i]);
        }
        if (_fromDataContainers != null)
        {
            foreach (var container in _fromDataContainers)
            {
                container.Value = null;
            }
        }
        return ret;
    }

    private object CastResult(object result, Type targetType)
    {
        if (result == null || result == DependencyProperty.UnsetValue || result == Binding.DoNothing || targetType == null || targetType == typeof(object))
        {
            return result!;
        }

        if (targetType == typeof(string))
        {
            return result.ToString()!;
        }

        if (!castFunctions.TryGetValue(targetType, out Func<object, object>? cast))
        {
            ParameterExpression par = Expression.Parameter(typeof(object));
            cast = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Dynamic(Binder.Convert(CSharpBinderFlags.ConvertExplicit, targetType, typeof(object)), targetType, par), typeof(object)), par).Compile();
            castFunctions.TryAdd(targetType, cast);
        }
        if (cast != null)
        {
            try
            {
                result = cast(result);
            }
            catch
            {
                castFunctions[targetType] = null!;
            }
        }
        return result;
    }
}
