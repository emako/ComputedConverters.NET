using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum TextBoxUnbindableCanWriteProperty
{
    SelectedText,
    SelectionLength,
    SelectionStart,
    CaretIndex
}

public class TextBoxSetStateToControlBehavior : Behavior<TextBox>
{
    public TextBoxSetStateToControlBehavior()
    {
    }

    public TextBoxUnbindableCanWriteProperty Property
    {
        get => (TextBoxUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(TextBoxUnbindableCanWriteProperty), typeof(TextBoxSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(TextBoxSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (TextBoxSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case TextBoxUnbindableCanWriteProperty.SelectedText:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((string)e.OldValue == (string)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.SelectedText = (string)thisObject.Source;
                break;

            case TextBoxUnbindableCanWriteProperty.SelectionLength:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((int)e.OldValue == (int)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.SelectionLength = (int)thisObject.Source;
                break;

            case TextBoxUnbindableCanWriteProperty.SelectionStart:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((int)e.OldValue == (int)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.SelectionStart = (int)thisObject.Source;
                break;

            case TextBoxUnbindableCanWriteProperty.CaretIndex:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((int)e.OldValue == (int)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.CaretIndex = (int)thisObject.Source;
                break;
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        //Attatch時の評価
        SourceChanged(this, new DependencyPropertyChangedEventArgs(SourceProperty, null, Source));
    }
}
