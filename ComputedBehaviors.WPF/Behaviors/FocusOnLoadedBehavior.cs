using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace ComputedBehaviors;

public sealed class FocusOnLoadedBehavior : Behavior<FrameworkElement>
{
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

    private void OnLoaded(object sender, EventArgs e)
    {
        AssociatedObject.Focus();
    }
}
