﻿using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ComputedAnimations.Extensions;

internal static class ListBoxExtensions
{
    internal static void Initialize(this ListBox lb)
    {
        lb.Loaded += AnimatedListBox_Loaded;

        void AnimatedListBox_Loaded(object sender, RoutedEventArgs e)
        {
            lb.Loaded -= AnimatedListBox_Loaded;

            // Perform validations on the ListBox
            Animations.Validate(lb);

            // Perform validations on the ListBox items
            Animations.ValidateListBox(lb);
        }
    }

    internal static void RegisterListEvents(this ListBox lb, Action itemsSourceChangedAction)
    {
        // Observe the Unloaded of the control (including Error or Completed)
        var unloaded = (lb as FrameworkElement).Events().Unloaded;

        lb.Observe(ItemsControl.ItemsSourceProperty)
            .TakeUntil(unloaded)
            // Animate only if the attached property is true
            .Where(_ => Animations.GetAnimateOnItemsSourceChange(lb))
            .Subscribe(
            _ => itemsSourceChangedAction?.Invoke(),
            ex => Debug.WriteLine($"Error on subscription to changes of the {nameof(ListBox.ItemsSource)} property of {nameof(ListBox)}", ex));
    }

    internal static void PrepareContainerForItemOverrideEx(this ListBox lb, DependencyObject element, ref bool isFirstItemContainerLoaded)
    {
        // Don't animate using this override if "on loaded" animations is false
        if (!Animations.GetAnimateOnLoad(lb))
        {
            return;
        }

        // Exit the element is not a ListBoxItem or if no animations are specified
        if (element is not ListBoxItem lbi || Animations.GetItems(lb) == null!)
        {
            return;
        }

        lb.AnimateItems(lbi, ref isFirstItemContainerLoaded);
    }

    internal static void AnimateItems(this ListBox lb, ListBoxItem item, ref bool isFirstItemContainerLoaded)
    {
        var settings = Animations.GetItems(lb);

        if (settings == null!)
        {
            return;
        }

        if (!isFirstItemContainerLoaded)
        {
            isFirstItemContainerLoaded = true;

            item.Loaded += OnContainerLoaded;

            // At this point, the index values are all ready to use.
            void OnContainerLoaded(object sender, RoutedEventArgs _)
            {
                ((ListBoxItem)sender).Loaded -= OnContainerLoaded;

                AnimateVisibleItems(lb, settings);
            }
        }
    }

    internal static void AnimateVisibleItems(this ListBox lb, AnimationSettings settings)
    {
        if (lb.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        {
            return;
        }

        // Make sure to retrieve the GetInterElementDelay value
        var interElementDelay = Animations.GetInterElementDelay(lb);

        var scroller = lb?.FindDescendant<ScrollViewer>();
        var visibleItems = scroller!.ViewportHeight;
        var top = scroller.VerticalOffset;
        var bottom = top + visibleItems;

        for (var index = top; index <= bottom; index++)
        {
            if (lb?.ItemContainerGenerator.ContainerFromIndex((int)index) is ListBoxItem lbi)
            {
                AnimateVisibleItem(lb, lbi, settings, interElementDelay);
            }
        }
    }

    private static void AnimateVisibleItem(ListBox lb, ListBoxItem lbi, AnimationSettings settings, double interElementDelay)
    {
        // According to MSDN: When CanContentScroll is true, the values of the ExtentHeight,
        // ScrollableHeight, ViewportHeight, and VerticalOffset properties are number of items.

        var scroller = lb?.FindDescendant<ScrollViewer>();
        var indexFromVisibleTop = lb!.ItemContainerGenerator.IndexFromContainer(lbi) - scroller!.VerticalOffset;

        // Create a clone of 'settings'
        var itemSettings = new AnimationSettings().ApplyOverrides(settings);

        itemSettings.Delay += indexFromVisibleTop * interElementDelay;

        Animations.RunAnimation(lbi, settings: itemSettings);
    }
}
