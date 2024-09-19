using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ComputedAnimations;

public static class AnimationEventFetch
{
    public static List<EventFetch> GetEventFetches(DependencyObject obj)
    {
        return (List<EventFetch>)obj.GetValue(EventFetchesProperty);
    }

    public static void SetEventFetches(DependencyObject obj, List<EventFetch> value)
    {
        obj.SetValue(EventFetchesProperty, value);
    }

    public static readonly DependencyProperty EventFetchesProperty = DependencyProperty.RegisterAttached("EventFetches", typeof(List<EventFetch>), typeof(AnimationEventFetch), new PropertyMetadata(null!));

    public static void AddEventHandler(DependencyObject d, string @event, Action action)
    {
        EventInfo? eventInfo = d.GetType().GetEvent(@event);
        if (eventInfo is null)
        {
            Debug.WriteLine("Event not found in target DependencyObject.");
            Debugger.Break();
            return;
        }

        Action<object, EventArgs> actionWrap = (_, _) => action?.Invoke();
        Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, actionWrap.Target, actionWrap.Method);

        eventInfo?.AddEventHandler(d, handler);
        List<EventFetch> fetches = GetEventFetches(d);

        if (fetches == null)
        {
            fetches = [];
            SetEventFetches(d, fetches);
        }
        fetches.Add(new EventFetch()
        {
            Action = action,
            Handler = handler,
        });
    }

    public static void RemoveEventHandler(DependencyObject d, string @event)
    {
        List<EventFetch> fetches = GetEventFetches(d);
        if (fetches == null || !fetches.Any())
        {
            return;
        }

        EventInfo? eventInfo = d.GetType().GetEvent(@event);
        foreach (EventFetch fetch in fetches)
        {
            eventInfo?.RemoveEventHandler(d, fetch.Handler);
        }
        SetEventFetches(d, null!);
    }
}

public class EventFetch
{
    public Action? Action { get; set; }
    public Delegate? Handler { get; set; }
}
