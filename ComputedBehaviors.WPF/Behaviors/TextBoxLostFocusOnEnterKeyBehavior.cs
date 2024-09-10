using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputedBehaviors;

public sealed class TextBoxLostFocusOnEnterKeyBehavior : Behavior<FrameworkElement>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AllowDrop = false;
        if (AssociatedObject is TextBox textBox)
        {
            textBox.AcceptsReturn = false;
        }
        AssociatedObject.PreviewKeyDown += OnKeyDown;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.PreviewKeyDown -= OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (AssociatedObject.IsFocused)
            {
                if (AssociatedObject.Parent is Panel parent)
                {
                    e.Handled = true;
                    Control dummyFocusControl = new();
                    parent.Children.Add(dummyFocusControl);
                    Keyboard.Focus(dummyFocusControl);
                    Keyboard.ClearFocus();
                    parent.Children.Remove(dummyFocusControl);
                }
            }
        }
    }
}
