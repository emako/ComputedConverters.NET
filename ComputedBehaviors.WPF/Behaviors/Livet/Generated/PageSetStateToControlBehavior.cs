using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum PageUnbindableCanWriteProperty
{
    WindowTitle,
    WindowHeight,
    WindowWidth,
    ShowsNavigationUI
}

public class PageSetStateToControlBehavior : Behavior<Page>
{
    public PageSetStateToControlBehavior()
    {
    }

    public PageUnbindableCanWriteProperty Property
    {
        get => (PageUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(PageUnbindableCanWriteProperty), typeof(PageSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(PageSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (PageSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case PageUnbindableCanWriteProperty.WindowTitle:
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
                thisObject.AssociatedObject.WindowTitle = (string)thisObject.Source;
                break;

            case PageUnbindableCanWriteProperty.WindowHeight:
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
                thisObject.AssociatedObject.WindowHeight = (double)thisObject.Source;
                break;

            case PageUnbindableCanWriteProperty.WindowWidth:
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
                thisObject.AssociatedObject.WindowWidth = (double)thisObject.Source;
                break;

            case PageUnbindableCanWriteProperty.ShowsNavigationUI:
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
                thisObject.AssociatedObject.ShowsNavigationUI = (bool)thisObject.Source;
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
