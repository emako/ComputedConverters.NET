using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace ComputedBehaviors;

public enum NavigationWindowUnbindableCanReadProperty
{
    BackStack,
    ForwardStack,
    CurrentSource,
    CanGoForward,
    CanGoBack,
    IsActive,
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

public class NavigationWindowSetStateToSourceAction : TriggerAction<NavigationWindow>
{
    public NavigationWindowSetStateToSourceAction()
    {
    }

    public NavigationWindowUnbindableCanReadProperty Property
    {
        get => (NavigationWindowUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(NavigationWindowUnbindableCanReadProperty), typeof(NavigationWindowSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(NavigationWindowSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case NavigationWindowUnbindableCanReadProperty.BackStack:
                if ((System.Collections.IEnumerable)Source != AssociatedObject.BackStack)
                {
                    Source = AssociatedObject.BackStack;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.ForwardStack:
                if ((System.Collections.IEnumerable)Source != AssociatedObject.ForwardStack)
                {
                    Source = AssociatedObject.ForwardStack;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.CurrentSource:
                if ((System.Uri)Source != AssociatedObject.CurrentSource)
                {
                    Source = AssociatedObject.CurrentSource;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.CanGoForward:
                if ((bool)Source != AssociatedObject.CanGoForward)
                {
                    Source = AssociatedObject.CanGoForward;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.CanGoBack:
                if ((bool)Source != AssociatedObject.CanGoBack)
                {
                    Source = AssociatedObject.CanGoBack;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsActive:
                if ((bool)Source != AssociatedObject.IsActive)
                {
                    Source = AssociatedObject.IsActive;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.HasContent:
                if ((bool)Source != AssociatedObject.HasContent)
                {
                    Source = AssociatedObject.HasContent;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case NavigationWindowUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
