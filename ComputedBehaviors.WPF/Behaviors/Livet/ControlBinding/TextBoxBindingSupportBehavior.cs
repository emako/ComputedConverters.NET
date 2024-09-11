using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public class TextBoxBindingSupportBehavior : Behavior<TextBox>
{
    public static readonly DependencyProperty SelectedTextProperty =
       DependencyProperty.Register(nameof(SelectedText), typeof(string), typeof(TextBoxBindingSupportBehavior),
           new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
               SourceSelectedTextChanged));

    public static readonly DependencyProperty SelectionLengthProperty =
       DependencyProperty.Register(nameof(SelectionLength), typeof(int), typeof(TextBoxBindingSupportBehavior),
           new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
               SourceSelectionLengthChanged));

    public static readonly DependencyProperty SelectionStartProperty =
       DependencyProperty.Register(nameof(SelectionStart), typeof(int), typeof(TextBoxBindingSupportBehavior),
           new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
               SourceSelectionStartChanged));

    public string SelectedText
    {
        get => (string)GetValue(SelectedTextProperty);
        set => SetValue(SelectedTextProperty, value);
    }

    public int SelectionLength
    {
        get => (int)(GetValue(SelectionLengthProperty) ?? default(int));
        set => SetValue(SelectionLengthProperty, value);
    }

    public int SelectionStart
    {
        get => (int)(GetValue(SelectionStartProperty) ?? default(int));
        set => SetValue(SelectionStartProperty, value);
    }

    private static void SourceSelectedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = sender as TextBoxBindingSupportBehavior;
        var associatedObject = thisObject?.AssociatedObject;
        var s = e.NewValue as string;
        if (associatedObject != null && associatedObject.SelectedText != s && s != null)
            associatedObject.SelectedText = s;
    }

    private static void SourceSelectionLengthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var associatedObject = (sender as TextBoxBindingSupportBehavior)?.AssociatedObject;
        if (associatedObject == null) return;

        var newValue = (int)(e.NewValue ?? default(int));
        if (associatedObject.SelectionLength != newValue)
            associatedObject.SelectionLength = newValue;
    }

    private static void SourceSelectionStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var associatedObject = (sender as TextBoxBindingSupportBehavior)?.AssociatedObject;
        if (associatedObject == null) return;

        var newValue = (int)(e.NewValue ?? default(int));
        if (associatedObject.SelectionStart != newValue)
            associatedObject.SelectionStart = newValue;
    }

    private void ControlSelectedTextChanged(object sender, RoutedEventArgs e)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        var textBox = (TextBox)sender;

        if (SelectedText != textBox.SelectedText) SelectedText = textBox.SelectedText;

        if (SelectionStart != textBox.SelectionStart) SelectionStart = textBox.SelectionStart;

        if (SelectionLength != textBox.SelectionLength) SelectionLength = textBox.SelectionLength;
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        SourceSelectedTextChanged(this,
            new DependencyPropertyChangedEventArgs(SelectedTextProperty, null, SelectedText));
        SourceSelectionStartChanged(this,
            new DependencyPropertyChangedEventArgs(SelectionStartProperty, null, SelectionStart));
        SourceSelectionLengthChanged(this,
            new DependencyPropertyChangedEventArgs(SelectionLengthProperty, null, SelectionLength));

        if (AssociatedObject != null) AssociatedObject.SelectionChanged += ControlSelectedTextChanged;
    }
}
