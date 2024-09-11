using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum DatePickerTextBoxUnbindableCanReadProperty
{
    SelectedText,
    SelectionLength,
    SelectionStart,
    CaretIndex,
    LineCount,
    ExtentWidth,
    ExtentHeight,
    ViewportWidth,
    ViewportHeight,
    HorizontalOffset,
    VerticalOffset,
    CanUndo,
    CanRedo,
    IsSelectionActive,
    ActualWidth,
    ActualHeight,
    IsInitialized,
    IsLoaded,
    HasAnimatedProperties,
    IsMeasureValid,
    IsArrangeValid,
    IsMouseDirectlyOver,
    IsMouseOver,
    IsStylusOver,
    IsKeyboardFocusWithin,
    IsMouseCaptured,
    IsMouseCaptureWithin,
    IsStylusDirectlyOver,
    IsStylusCaptured,
    IsStylusCaptureWithin,
    IsKeyboardFocused,
    IsInputMethodEnabled,
    IsFocused,
    IsVisible,
    AreAnyTouchesOver,
    AreAnyTouchesDirectlyOver,
    AreAnyTouchesCapturedWithin,
    AreAnyTouchesCaptured,
    IsSealed
}

public class DatePickerTextBoxSetStateToSourceAction : TriggerAction<DatePickerTextBox>
{
    public DatePickerTextBoxSetStateToSourceAction()
    {
    }

    public DatePickerTextBoxUnbindableCanReadProperty Property
    {
        get => (DatePickerTextBoxUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(DatePickerTextBoxUnbindableCanReadProperty), typeof(DatePickerTextBoxSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(DatePickerTextBoxSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case DatePickerTextBoxUnbindableCanReadProperty.SelectedText:
                if ((string)Source != AssociatedObject.SelectedText)
                {
                    Source = AssociatedObject.SelectedText;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.SelectionLength:
                if ((int)Source != AssociatedObject.SelectionLength)
                {
                    Source = AssociatedObject.SelectionLength;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.SelectionStart:
                if ((int)Source != AssociatedObject.SelectionStart)
                {
                    Source = AssociatedObject.SelectionStart;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.CaretIndex:
                if ((int)Source != AssociatedObject.CaretIndex)
                {
                    Source = AssociatedObject.CaretIndex;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.LineCount:
                if ((int)Source != AssociatedObject.LineCount)
                {
                    Source = AssociatedObject.LineCount;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ExtentWidth:
                if ((double)Source != AssociatedObject.ExtentWidth)
                {
                    Source = AssociatedObject.ExtentWidth;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ExtentHeight:
                if ((double)Source != AssociatedObject.ExtentHeight)
                {
                    Source = AssociatedObject.ExtentHeight;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ViewportWidth:
                if ((double)Source != AssociatedObject.ViewportWidth)
                {
                    Source = AssociatedObject.ViewportWidth;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ViewportHeight:
                if ((double)Source != AssociatedObject.ViewportHeight)
                {
                    Source = AssociatedObject.ViewportHeight;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.HorizontalOffset:
                if ((double)Source != AssociatedObject.HorizontalOffset)
                {
                    Source = AssociatedObject.HorizontalOffset;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.VerticalOffset:
                if ((double)Source != AssociatedObject.VerticalOffset)
                {
                    Source = AssociatedObject.VerticalOffset;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.CanUndo:
                if ((bool)Source != AssociatedObject.CanUndo)
                {
                    Source = AssociatedObject.CanUndo;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.CanRedo:
                if ((bool)Source != AssociatedObject.CanRedo)
                {
                    Source = AssociatedObject.CanRedo;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsSelectionActive:
                if ((bool)Source != AssociatedObject.IsSelectionActive)
                {
                    Source = AssociatedObject.IsSelectionActive;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case DatePickerTextBoxUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
