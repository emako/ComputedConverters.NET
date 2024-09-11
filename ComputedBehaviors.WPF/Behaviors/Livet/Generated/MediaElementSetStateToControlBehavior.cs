using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum MediaElementUnbindableCanWriteProperty
{
    Position,
    SpeedRatio
}

public class MediaElementSetStateToControlBehavior : Behavior<MediaElement>
{
    public MediaElementSetStateToControlBehavior()
    {
    }

    public MediaElementUnbindableCanWriteProperty Property
    {
        get => (MediaElementUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(MediaElementUnbindableCanWriteProperty), typeof(MediaElementSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(MediaElementSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (MediaElementSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case MediaElementUnbindableCanWriteProperty.Position:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((System.TimeSpan)e.OldValue == (System.TimeSpan)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.Position = (System.TimeSpan)thisObject.Source;
                break;

            case MediaElementUnbindableCanWriteProperty.SpeedRatio:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((double)e.OldValue == (double)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.SpeedRatio = (double)thisObject.Source;
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
