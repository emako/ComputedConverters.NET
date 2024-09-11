using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum CalendarDayButtonUnbindableCanReadProperty
{
    IsToday,
    IsSelected,
    IsInactive,
    IsBlackedOut,
    IsHighlighted,
    IsDefaulted,
    IsPressed,
    HasContent,
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

public class CalendarDayButtonSetStateToSourceAction : TriggerAction<CalendarDayButton>
{
    public CalendarDayButtonSetStateToSourceAction()
    {
    }

    public CalendarDayButtonUnbindableCanReadProperty Property
    {
        get => (CalendarDayButtonUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(CalendarDayButtonUnbindableCanReadProperty), typeof(CalendarDayButtonSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(CalendarDayButtonSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case CalendarDayButtonUnbindableCanReadProperty.IsToday:
                if ((bool)Source != AssociatedObject.IsToday)
                {
                    Source = AssociatedObject.IsToday;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsSelected:
                if ((bool)Source != AssociatedObject.IsSelected)
                {
                    Source = AssociatedObject.IsSelected;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsInactive:
                if ((bool)Source != AssociatedObject.IsInactive)
                {
                    Source = AssociatedObject.IsInactive;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsBlackedOut:
                if ((bool)Source != AssociatedObject.IsBlackedOut)
                {
                    Source = AssociatedObject.IsBlackedOut;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsHighlighted:
                if ((bool)Source != AssociatedObject.IsHighlighted)
                {
                    Source = AssociatedObject.IsHighlighted;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsDefaulted:
                if ((bool)Source != AssociatedObject.IsDefaulted)
                {
                    Source = AssociatedObject.IsDefaulted;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsPressed:
                if ((bool)Source != AssociatedObject.IsPressed)
                {
                    Source = AssociatedObject.IsPressed;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.HasContent:
                if ((bool)Source != AssociatedObject.HasContent)
                {
                    Source = AssociatedObject.HasContent;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case CalendarDayButtonUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
