using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum FlowDocumentPageViewerUnbindableCanReadProperty
{
    CanIncreaseZoom,
    CanDecreaseZoom,
    IsSelectionActive,
    PageCount,
    MasterPageNumber,
    CanGoToPreviousPage,
    CanGoToNextPage,
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

public class FlowDocumentPageViewerSetStateToSourceAction : TriggerAction<FlowDocumentPageViewer>
{
    public FlowDocumentPageViewerSetStateToSourceAction()
    {
    }

    public FlowDocumentPageViewerUnbindableCanReadProperty Property
    {
        get => (FlowDocumentPageViewerUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(FlowDocumentPageViewerUnbindableCanReadProperty), typeof(FlowDocumentPageViewerSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(FlowDocumentPageViewerSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case FlowDocumentPageViewerUnbindableCanReadProperty.CanIncreaseZoom:
                if ((bool)Source != AssociatedObject.CanIncreaseZoom)
                {
                    Source = AssociatedObject.CanIncreaseZoom;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.CanDecreaseZoom:
                if ((bool)Source != AssociatedObject.CanDecreaseZoom)
                {
                    Source = AssociatedObject.CanDecreaseZoom;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsSelectionActive:
                if ((bool)Source != AssociatedObject.IsSelectionActive)
                {
                    Source = AssociatedObject.IsSelectionActive;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.PageCount:
                if ((int)Source != AssociatedObject.PageCount)
                {
                    Source = AssociatedObject.PageCount;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.MasterPageNumber:
                if ((int)Source != AssociatedObject.MasterPageNumber)
                {
                    Source = AssociatedObject.MasterPageNumber;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.CanGoToPreviousPage:
                if ((bool)Source != AssociatedObject.CanGoToPreviousPage)
                {
                    Source = AssociatedObject.CanGoToPreviousPage;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.CanGoToNextPage:
                if ((bool)Source != AssociatedObject.CanGoToNextPage)
                {
                    Source = AssociatedObject.CanGoToNextPage;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case FlowDocumentPageViewerUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
