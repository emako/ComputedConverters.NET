using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum GridUnbindableCanReadProperty
{
    HasLogicalOrientationPublic,
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

public class GridSetStateToSourceAction : TriggerAction<Grid>
{
    public GridSetStateToSourceAction()
    {
    }

    public GridUnbindableCanReadProperty Property
    {
        get => (GridUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(GridUnbindableCanReadProperty), typeof(GridSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(GridSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case GridUnbindableCanReadProperty.HasLogicalOrientationPublic:
                if ((bool)Source != AssociatedObject.HasLogicalOrientationPublic)
                {
                    Source = AssociatedObject.HasLogicalOrientationPublic;
                }
                break;

            case GridUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case GridUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case GridUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case GridUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case GridUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case GridUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case GridUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case GridUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case GridUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case GridUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case GridUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case GridUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case GridUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case GridUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case GridUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case GridUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case GridUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case GridUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case GridUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case GridUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case GridUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case GridUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case GridUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case GridUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case GridUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
