using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public enum ToolBarPanelUnbindableCanWriteProperty
{
    CanHorizontallyScroll,
    CanVerticallyScroll
}

public class ToolBarPanelSetStateToControlBehavior : Behavior<ToolBarPanel>
{
    public ToolBarPanelSetStateToControlBehavior()
    {
    }

    public ToolBarPanelUnbindableCanWriteProperty Property
    {
        get => (ToolBarPanelUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(ToolBarPanelUnbindableCanWriteProperty), typeof(ToolBarPanelSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(ToolBarPanelSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (ToolBarPanelSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case ToolBarPanelUnbindableCanWriteProperty.CanHorizontallyScroll:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((bool)e.OldValue == (bool)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.CanHorizontallyScroll = (bool)thisObject.Source;
                break;

            case ToolBarPanelUnbindableCanWriteProperty.CanVerticallyScroll:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((bool)e.OldValue == (bool)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.CanVerticallyScroll = (bool)thisObject.Source;
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
