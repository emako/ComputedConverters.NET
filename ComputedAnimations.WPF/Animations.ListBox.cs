﻿using System.Windows;
using System.Windows.Controls;
using ComputedAnimations.Extensions;

namespace ComputedAnimations;

public static partial class Animations
{
    public static bool GetItemsBinding(ListBox obj) => (bool)obj.GetValue(ItemsBindingProperty);

    public static void SetItemsBinding(ListBox obj, bool value) => obj.SetValue(ItemsBindingProperty, value);

    public static readonly DependencyProperty ItemsBindingProperty =
        DependencyProperty.RegisterAttached(
            "ItemsBinding",
            typeof(bool),
            typeof(Animations),
            new PropertyMetadata(false, OnItemsBindingChanged));

    public static AnimationSettings GetItems(ListBox obj) => (AnimationSettings)obj.GetValue(ItemsProperty);

    public static void SetItems(ListBox obj, AnimationSettings value) => obj.SetValue(ItemsProperty, value);

    /// <summary>
    /// The List animation intended to run on List's Loaded event or a value change on ItemsBinding.
    /// </summary>
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.RegisterAttached(
            "Items",
            typeof(AnimationSettings),
            typeof(Animations),
            new PropertyMetadata(null, OnItemsChanged));

    public static double GetInterElementDelay(ListBox obj) => (double)obj.GetValue(InterElementDelayProperty);

    public static void SetInterElementDelay(ListBox obj, double value) => obj.SetValue(InterElementDelayProperty, value);

    /// <summary>
    /// Specifies the animation delay to apply to every lb item.
    /// </summary>
    /// <remarks>NOTE: Delay is being overwritten by InterElementDelay. Therefore, a lb animation currently can't have an initial Delay.</remarks>
    public static readonly DependencyProperty InterElementDelayProperty =
        DependencyProperty.RegisterAttached(
            "InterElementDelay",
            typeof(double),
            typeof(Animations),
            new PropertyMetadata(DefaultSettings.InterElementDelay));

    public static bool GetAnimateOnLoad(ListBox obj) => (bool)obj.GetValue(AnimateOnLoadProperty);

    public static void SetAnimateOnLoad(ListBox obj, bool value) => obj.SetValue(AnimateOnLoadProperty, value);

    /// <summary>
    /// Specifies if the item animations should animate when the list control has loaded
    /// </summary>
    public static readonly DependencyProperty AnimateOnLoadProperty =
        DependencyProperty.RegisterAttached(
            "AnimateOnLoad",
            typeof(bool),
            typeof(Animations),
            new PropertyMetadata(true));

    public static bool GetAnimateOnItemsSourceChange(ListBox obj) => (bool)obj.GetValue(AnimateOnItemsSourceChangeProperty);

    public static void SetAnimateOnItemsSourceChange(ListBox obj, bool value) => obj.SetValue(AnimateOnItemsSourceChangeProperty, value);

    /// <summary>
    /// Specifies if the item animations should re-animate when there is a change on ItemsSource
    /// </summary>
    public static readonly DependencyProperty AnimateOnItemsSourceChangeProperty =
        DependencyProperty.RegisterAttached(
            "AnimateOnItemsSourceChange",
            typeof(bool),
            typeof(Animations),
            new PropertyMetadata(true));

    // TODO: NOT WORKING... ListBox.ItemContainerGenerator.ContainerFromIndex(...)
    // cannot fetch an item not within view
    // ------------------------------------
    //public static short GetVisibleThreshold(ListBox obj) => (short)obj.GetValue(VisibleThresholdProperty);

    //public static void SetVisibleThreshold(ListBox obj, short value) => obj.SetValue(VisibleThresholdProperty, value);

    ///// <summary>
    ///// Specifies the number of items out-of-view to include when animating
    ///// </summary>
    //public static readonly DependencyProperty VisibleThresholdProperty =
    //    DependencyProperty.RegisterAttached(
    //        "VisibleThreshold",
    //        typeof(short),
    //        typeof(Animations2),
    //        new PropertyMetadata((short)10));
    // ------------------------------------

    private static void OnItemsBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Prevent running animations in a Visual Designer
        if (IsInDesignMode(d))
        {
            return;
        }

        if (d is ListBox lb
            && lb.Items.Count > 0
            && e.NewValue is bool isAnimating
            && isAnimating)
        {
            var settings = GetItems(lb);

            lb.AnimateVisibleItems(settings);
        }
    }

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListBox lb)
        {
            lb.Initialize();
        }
    }
}
