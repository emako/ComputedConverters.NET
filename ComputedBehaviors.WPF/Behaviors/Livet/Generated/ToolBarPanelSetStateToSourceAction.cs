using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum ToolBarPanelUnbindableCanReadProperty
{
    CanHorizontallyScroll,
    CanVerticallyScroll,
    ExtentWidth,
    ExtentHeight,
    ViewportWidth,
    ViewportHeight,
    HorizontalOffset,
    VerticalOffset,
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

public class ToolBarPanelSetStateToSourceAction : TriggerAction<ToolBarPanel>
{
    public ToolBarPanelSetStateToSourceAction()
    {
    }

    public ToolBarPanelUnbindableCanReadProperty Property
    {
        get => (ToolBarPanelUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(ToolBarPanelUnbindableCanReadProperty), typeof(ToolBarPanelSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(ToolBarPanelSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case ToolBarPanelUnbindableCanReadProperty.CanHorizontallyScroll:
                if ((bool)Source != AssociatedObject.CanHorizontallyScroll)
                {
                    Source = AssociatedObject.CanHorizontallyScroll;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.CanVerticallyScroll:
                if ((bool)Source != AssociatedObject.CanVerticallyScroll)
                {
                    Source = AssociatedObject.CanVerticallyScroll;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ExtentWidth:
                if ((double)Source != AssociatedObject.ExtentWidth)
                {
                    Source = AssociatedObject.ExtentWidth;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ExtentHeight:
                if ((double)Source != AssociatedObject.ExtentHeight)
                {
                    Source = AssociatedObject.ExtentHeight;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ViewportWidth:
                if ((double)Source != AssociatedObject.ViewportWidth)
                {
                    Source = AssociatedObject.ViewportWidth;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ViewportHeight:
                if ((double)Source != AssociatedObject.ViewportHeight)
                {
                    Source = AssociatedObject.ViewportHeight;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.HorizontalOffset:
                if ((double)Source != AssociatedObject.HorizontalOffset)
                {
                    Source = AssociatedObject.HorizontalOffset;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.VerticalOffset:
                if ((double)Source != AssociatedObject.VerticalOffset)
                {
                    Source = AssociatedObject.VerticalOffset;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.HasLogicalOrientationPublic:
                if ((bool)Source != AssociatedObject.HasLogicalOrientationPublic)
                {
                    Source = AssociatedObject.HasLogicalOrientationPublic;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case ToolBarPanelUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
