using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public class PasswordBoxBindingSupportBehavior : Behavior<PasswordBox>
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBoxBindingSupportBehavior),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                SourcePasswordChanged));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    private static void SourcePasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var associatedObject = (sender as PasswordBoxBindingSupportBehavior)?.AssociatedObject;
        if (associatedObject == null) return;

        var newValue = e.NewValue as string;
        if (associatedObject.Password != newValue)
            associatedObject.Password = newValue ?? string.Empty;
    }

    private void ControlPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        var pb = (PasswordBox)sender;

        if (Password != pb.Password) Password = pb.Password;
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        SourcePasswordChanged(this, new DependencyPropertyChangedEventArgs(PasswordProperty, null, Password));

        if (AssociatedObject != null) AssociatedObject.PasswordChanged += ControlPasswordChanged;
    }
}
