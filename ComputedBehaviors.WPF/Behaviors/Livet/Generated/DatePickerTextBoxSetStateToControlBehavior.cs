using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum DatePickerTextBoxUnbindableCanWriteProperty
{
    SelectedText,
    SelectionLength,
    SelectionStart,
    CaretIndex
}

public class DatePickerTextBoxSetStateToControlBehavior : Behavior<DatePickerTextBox>
{
    public DatePickerTextBoxSetStateToControlBehavior()
    {
    }

    public DatePickerTextBoxUnbindableCanWriteProperty Property
    {
        get => (DatePickerTextBoxUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(DatePickerTextBoxUnbindableCanWriteProperty), typeof(DatePickerTextBoxSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(DatePickerTextBoxSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (DatePickerTextBoxSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case DatePickerTextBoxUnbindableCanWriteProperty.SelectedText:
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

            case DatePickerTextBoxUnbindableCanWriteProperty.SelectionLength:
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

            case DatePickerTextBoxUnbindableCanWriteProperty.SelectionStart:
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

            case DatePickerTextBoxUnbindableCanWriteProperty.CaretIndex:
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
