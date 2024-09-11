using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum DataGridRowsPresenterUnbindableCanReadProperty
{
    CanHorizontallyScroll,
    CanVerticallyScroll,
    ExtentWidth,
    ExtentHeight,
    ViewportWidth,
    ViewportHeight,
    HorizontalOffset,
    VerticalOffset,
    CanHierarchicallyScrollAndVirtualize,
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

public class DataGridRowsPresenterSetStateToSourceAction : TriggerAction<DataGridRowsPresenter>
{
    public DataGridRowsPresenterSetStateToSourceAction()
    {
    }

    public DataGridRowsPresenterUnbindableCanReadProperty Property
    {
        get => (DataGridRowsPresenterUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(DataGridRowsPresenterUnbindableCanReadProperty), typeof(DataGridRowsPresenterSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(DataGridRowsPresenterSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case DataGridRowsPresenterUnbindableCanReadProperty.CanHorizontallyScroll:
                if ((bool)Source != AssociatedObject.CanHorizontallyScroll)
                {
                    Source = AssociatedObject.CanHorizontallyScroll;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.CanVerticallyScroll:
                if ((bool)Source != AssociatedObject.CanVerticallyScroll)
                {
                    Source = AssociatedObject.CanVerticallyScroll;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ExtentWidth:
                if ((double)Source != AssociatedObject.ExtentWidth)
                {
                    Source = AssociatedObject.ExtentWidth;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ExtentHeight:
                if ((double)Source != AssociatedObject.ExtentHeight)
                {
                    Source = AssociatedObject.ExtentHeight;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ViewportWidth:
                if ((double)Source != AssociatedObject.ViewportWidth)
                {
                    Source = AssociatedObject.ViewportWidth;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ViewportHeight:
                if ((double)Source != AssociatedObject.ViewportHeight)
                {
                    Source = AssociatedObject.ViewportHeight;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.HorizontalOffset:
                if ((double)Source != AssociatedObject.HorizontalOffset)
                {
                    Source = AssociatedObject.HorizontalOffset;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.VerticalOffset:
                if ((double)Source != AssociatedObject.VerticalOffset)
                {
                    Source = AssociatedObject.VerticalOffset;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.CanHierarchicallyScrollAndVirtualize:
                if ((bool)Source != AssociatedObject.CanHierarchicallyScrollAndVirtualize)
                {
                    Source = AssociatedObject.CanHierarchicallyScrollAndVirtualize;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.HasLogicalOrientationPublic:
                if ((bool)Source != AssociatedObject.HasLogicalOrientationPublic)
                {
                    Source = AssociatedObject.HasLogicalOrientationPublic;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case DataGridRowsPresenterUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
