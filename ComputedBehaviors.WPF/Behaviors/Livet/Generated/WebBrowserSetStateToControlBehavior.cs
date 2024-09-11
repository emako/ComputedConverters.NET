using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public enum WebBrowserUnbindableCanWriteProperty
{
    Source,
    ObjectForScripting
}

public class WebBrowserSetStateToControlBehavior : Behavior<WebBrowser>
{
    public WebBrowserSetStateToControlBehavior()
    {
    }

    public WebBrowserUnbindableCanWriteProperty Property
    {
        get => (WebBrowserUnbindableCanWriteProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty =
        DependencyProperty.Register(nameof(Property), typeof(WebBrowserUnbindableCanWriteProperty), typeof(WebBrowserSetStateToControlBehavior), new PropertyMetadata());

    [Bindable(BindableSupport.Default, BindingDirection.OneWay)]
    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(WebBrowserSetStateToControlBehavior), new PropertyMetadata(null, new PropertyChangedCallback(SourceChanged)));

    private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var thisObject = (WebBrowserSetStateToControlBehavior)sender;

        if (thisObject.AssociatedObject == null)
        {
            return;
        }

        switch (thisObject.Property)
        {
            case WebBrowserUnbindableCanWriteProperty.Source:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if ((System.Uri)e.OldValue == (System.Uri)e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.Source = (System.Uri)thisObject.Source;
                break;

            case WebBrowserUnbindableCanWriteProperty.ObjectForScripting:
                if (e.NewValue == null)
                {
                    return;
                }
                if (e.OldValue != null)
                {
                    if (e.OldValue == e.NewValue)
                    {
                        return;
                    }
                }
                thisObject.AssociatedObject.ObjectForScripting = thisObject.Source;
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
