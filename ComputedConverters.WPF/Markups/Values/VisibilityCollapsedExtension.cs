﻿using System;
using System.Windows;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Visibility))]
public sealed class VisibilityCollapsedExtension() : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Visibility.Collapsed;
    }
}