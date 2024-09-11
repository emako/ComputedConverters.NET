using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum DocumentViewerUnbindableCanReadProperty
{
    ExtentWidth,
    ExtentHeight,
    ViewportWidth,
    ViewportHeight,
    CanMoveUp,
    CanMoveDown,
    CanMoveLeft,
    CanMoveRight,
    CanIncreaseZoom,
    CanDecreaseZoom,
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

public class DocumentViewerSetStateToSourceAction : TriggerAction<DocumentViewer>
{
    public DocumentViewerSetStateToSourceAction()
    {
    }

    public DocumentViewerUnbindableCanReadProperty Property
    {
        get => (DocumentViewerUnbindableCanReadProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(DocumentViewerUnbindableCanReadProperty), typeof(DocumentViewerSetStateToSourceAction), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(DocumentViewerSetStateToSourceAction), new PropertyMetadata(null));

    protected override void Invoke(object parameter)
    {
        switch (Property)
        {
            case DocumentViewerUnbindableCanReadProperty.ExtentWidth:
                if ((double)Source != AssociatedObject.ExtentWidth)
                {
                    Source = AssociatedObject.ExtentWidth;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.ExtentHeight:
                if ((double)Source != AssociatedObject.ExtentHeight)
                {
                    Source = AssociatedObject.ExtentHeight;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.ViewportWidth:
                if ((double)Source != AssociatedObject.ViewportWidth)
                {
                    Source = AssociatedObject.ViewportWidth;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.ViewportHeight:
                if ((double)Source != AssociatedObject.ViewportHeight)
                {
                    Source = AssociatedObject.ViewportHeight;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanMoveUp:
                if ((bool)Source != AssociatedObject.CanMoveUp)
                {
                    Source = AssociatedObject.CanMoveUp;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanMoveDown:
                if ((bool)Source != AssociatedObject.CanMoveDown)
                {
                    Source = AssociatedObject.CanMoveDown;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanMoveLeft:
                if ((bool)Source != AssociatedObject.CanMoveLeft)
                {
                    Source = AssociatedObject.CanMoveLeft;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanMoveRight:
                if ((bool)Source != AssociatedObject.CanMoveRight)
                {
                    Source = AssociatedObject.CanMoveRight;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanIncreaseZoom:
                if ((bool)Source != AssociatedObject.CanIncreaseZoom)
                {
                    Source = AssociatedObject.CanIncreaseZoom;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanDecreaseZoom:
                if ((bool)Source != AssociatedObject.CanDecreaseZoom)
                {
                    Source = AssociatedObject.CanDecreaseZoom;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.PageCount:
                if ((int)Source != AssociatedObject.PageCount)
                {
                    Source = AssociatedObject.PageCount;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.MasterPageNumber:
                if ((int)Source != AssociatedObject.MasterPageNumber)
                {
                    Source = AssociatedObject.MasterPageNumber;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanGoToPreviousPage:
                if ((bool)Source != AssociatedObject.CanGoToPreviousPage)
                {
                    Source = AssociatedObject.CanGoToPreviousPage;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.CanGoToNextPage:
                if ((bool)Source != AssociatedObject.CanGoToNextPage)
                {
                    Source = AssociatedObject.CanGoToNextPage;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.ActualWidth:
                if ((double)Source != AssociatedObject.ActualWidth)
                {
                    Source = AssociatedObject.ActualWidth;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.ActualHeight:
                if ((double)Source != AssociatedObject.ActualHeight)
                {
                    Source = AssociatedObject.ActualHeight;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsInitialized:
                if ((bool)Source != AssociatedObject.IsInitialized)
                {
                    Source = AssociatedObject.IsInitialized;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsLoaded:
                if ((bool)Source != AssociatedObject.IsLoaded)
                {
                    Source = AssociatedObject.IsLoaded;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.HasAnimatedProperties:
                if ((bool)Source != AssociatedObject.HasAnimatedProperties)
                {
                    Source = AssociatedObject.HasAnimatedProperties;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsMeasureValid:
                if ((bool)Source != AssociatedObject.IsMeasureValid)
                {
                    Source = AssociatedObject.IsMeasureValid;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsArrangeValid:
                if ((bool)Source != AssociatedObject.IsArrangeValid)
                {
                    Source = AssociatedObject.IsArrangeValid;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsMouseDirectlyOver:
                if ((bool)Source != AssociatedObject.IsMouseDirectlyOver)
                {
                    Source = AssociatedObject.IsMouseDirectlyOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsMouseOver:
                if ((bool)Source != AssociatedObject.IsMouseOver)
                {
                    Source = AssociatedObject.IsMouseOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsStylusOver:
                if ((bool)Source != AssociatedObject.IsStylusOver)
                {
                    Source = AssociatedObject.IsStylusOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsKeyboardFocusWithin:
                if ((bool)Source != AssociatedObject.IsKeyboardFocusWithin)
                {
                    Source = AssociatedObject.IsKeyboardFocusWithin;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsMouseCaptured:
                if ((bool)Source != AssociatedObject.IsMouseCaptured)
                {
                    Source = AssociatedObject.IsMouseCaptured;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsMouseCaptureWithin:
                if ((bool)Source != AssociatedObject.IsMouseCaptureWithin)
                {
                    Source = AssociatedObject.IsMouseCaptureWithin;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsStylusDirectlyOver:
                if ((bool)Source != AssociatedObject.IsStylusDirectlyOver)
                {
                    Source = AssociatedObject.IsStylusDirectlyOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsStylusCaptured:
                if ((bool)Source != AssociatedObject.IsStylusCaptured)
                {
                    Source = AssociatedObject.IsStylusCaptured;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsStylusCaptureWithin:
                if ((bool)Source != AssociatedObject.IsStylusCaptureWithin)
                {
                    Source = AssociatedObject.IsStylusCaptureWithin;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsKeyboardFocused:
                if ((bool)Source != AssociatedObject.IsKeyboardFocused)
                {
                    Source = AssociatedObject.IsKeyboardFocused;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsInputMethodEnabled:
                if ((bool)Source != AssociatedObject.IsInputMethodEnabled)
                {
                    Source = AssociatedObject.IsInputMethodEnabled;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsFocused:
                if ((bool)Source != AssociatedObject.IsFocused)
                {
                    Source = AssociatedObject.IsFocused;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsVisible:
                if ((bool)Source != AssociatedObject.IsVisible)
                {
                    Source = AssociatedObject.IsVisible;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.AreAnyTouchesOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesOver)
                {
                    Source = AssociatedObject.AreAnyTouchesOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.AreAnyTouchesDirectlyOver:
                if ((bool)Source != AssociatedObject.AreAnyTouchesDirectlyOver)
                {
                    Source = AssociatedObject.AreAnyTouchesDirectlyOver;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.AreAnyTouchesCapturedWithin:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCapturedWithin)
                {
                    Source = AssociatedObject.AreAnyTouchesCapturedWithin;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.AreAnyTouchesCaptured:
                if ((bool)Source != AssociatedObject.AreAnyTouchesCaptured)
                {
                    Source = AssociatedObject.AreAnyTouchesCaptured;
                }
                break;

            case DocumentViewerUnbindableCanReadProperty.IsSealed:
                if ((bool)Source != AssociatedObject.IsSealed)
                {
                    Source = AssociatedObject.IsSealed;
                }
                break;
        }
    }
}
