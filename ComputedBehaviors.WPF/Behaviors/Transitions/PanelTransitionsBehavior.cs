using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ComputedBehaviors;

public sealed class PanelTransitionsBehavior : Behavior<Panel>
{
    public PanelTransitionsBehaviorType Type { get; set; } = PanelTransitionsBehaviorType.DownToUp;
    public bool IsOnce { get; set; } = false;
    public double BeginTimeDelay { get; set; } = 0d;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += OnLoaded;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (IsOnce)
        {
            AssociatedObject.Loaded -= OnLoaded;
        }
        if (AssociatedObject != null)
        {
            if (Type == PanelTransitionsBehaviorType.DownToUp)
            {
                int i = default;
                foreach (UIElement child in AssociatedObject.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        Storyboard sb = new();
                        DoubleAnimation fromVerticalOffsetAnimaiton = new()
                        {
                            To = 0d,
                            BeginTime = TimeSpan.FromSeconds(i / 40d + BeginTimeDelay),
                            Duration = new Duration(TimeSpan.FromSeconds(0.3d)),
                            From = 15d,
                            EasingFunction = new SineEase() { EasingMode = EasingMode.EaseIn },
                        };
                        DoubleAnimation fadeInAnimation = new()
                        {
                            To = 1d,
                            BeginTime = TimeSpan.FromSeconds(i / 40d + BeginTimeDelay),
                            Duration = new Duration(TimeSpan.FromSeconds(0.4d)),
                            From = 0d,
                            EasingFunction = new SineEase() { EasingMode = EasingMode.EaseIn },
                        };
                        i++;

                        child.Opacity = 0d;
                        child.RenderTransform = new TranslateTransform() { Y = 15d };
                        Storyboard.SetTarget(fromVerticalOffsetAnimaiton, element);
                        Storyboard.SetTarget(fadeInAnimation, element);
                        Storyboard.SetTargetProperty(fromVerticalOffsetAnimaiton, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                        Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
                        sb.Children.Add(fromVerticalOffsetAnimaiton);
                        sb.Children.Add(fadeInAnimation);
                        sb.Begin();
                    }
                }
            }
            else
            {
                int i = default;
                foreach (UIElement child in AssociatedObject.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        Storyboard sb = new();
                        DoubleAnimation fromHorizontalOffsetAnimaiton = new()
                        {
                            To = 0d,
                            BeginTime = TimeSpan.FromSeconds(i / 40d + BeginTimeDelay),
                            Duration = new Duration(TimeSpan.FromSeconds(0.3d)),
                            From = 15d,
                            EasingFunction = new SineEase() { EasingMode = EasingMode.EaseIn },
                        };
                        DoubleAnimation fadeInAnimation = new()
                        {
                            To = 1d,
                            BeginTime = TimeSpan.FromSeconds(i / 40d + BeginTimeDelay),
                            Duration = new Duration(TimeSpan.FromSeconds(0.4d)),
                            From = 0d,
                            EasingFunction = new SineEase() { EasingMode = EasingMode.EaseIn },
                        };
                        i++;

                        child.Opacity = 0d;
                        child.RenderTransform = new TranslateTransform() { X = 15d };
                        Storyboard.SetTarget(fromHorizontalOffsetAnimaiton, element);
                        Storyboard.SetTarget(fadeInAnimation, element);
                        Storyboard.SetTargetProperty(fromHorizontalOffsetAnimaiton, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                        Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
                        sb.Children.Add(fromHorizontalOffsetAnimaiton);
                        sb.Children.Add(fadeInAnimation);
                        sb.Begin();
                    }
                }
            }
        }
    }
}

public enum PanelTransitionsBehaviorType
{
    DownToUp,
    RightToLeft,
}
