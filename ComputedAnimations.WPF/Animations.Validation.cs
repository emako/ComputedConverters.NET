using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ComputedAnimations.Controls;
using ComputedAnimations.Controlsl;
using ComputedAnimations.Extensions;

#pragma warning disable CS8604

namespace ComputedAnimations;

public static partial class Animations
{
    internal static void Validate(FrameworkElement element)
    {
        // Skip validation if a debugger isn't attached
        if (!Debugger.IsAttached)
        {
            return;
        }

        var startWith = element.ReadLocalValue(StartWithProperty);
        var primaryIsSet = (element.ReadLocalValue(PrimaryProperty) != DependencyProperty.UnsetValue);
        var primaryBindingIsSet = (element.ReadLocalValue(PrimaryBindingProperty) != DependencyProperty.UnsetValue);
        var secondaryIsSet = (element.ReadLocalValue(SecondaryProperty) != DependencyProperty.UnsetValue);
        var secondaryBindingIsSet = (element.ReadLocalValue(SecondaryBindingProperty) != DependencyProperty.UnsetValue);
        var combinedBindingIsSet = (element.ReadLocalValue(CombinedBindingProperty) != DependencyProperty.UnsetValue);
        var isIterating = GetIterationBehavior(element) == IterationBehavior.Forever || GetIterationCount(element) > 1;

        AnimationSettings primarySettings = null!;
        CompoundSettings primaryCompoundSettings = null!;
        AnimationSettings secondarySettings = null!;
        CompoundSettings secondaryCompoundSettings = null!;

        if (GetPrimary(element) is CompoundSettings compoundPrimary)
        {
            primaryCompoundSettings = compoundPrimary;
        }
        else
        {
            primarySettings = (GetPrimary(element) as AnimationSettings)!;
        }

        if (GetSecondary(element) is CompoundSettings compoundSecondary)
        {
            secondaryCompoundSettings = compoundSecondary;
        }
        else
        {
            secondarySettings = (GetSecondary(element) as AnimationSettings)!;
        }

        // Cannot set an animation for Secondary when specifying values for IterationCount or IterationBehavior.
        if (isIterating && secondaryIsSet)
        {
            throw new ArgumentException($"Cannot set an animation for {nameof(SecondaryProperty)} when specifying values for {nameof(IterationCountProperty)} or {nameof(IterationBehaviorProperty)}.");
        }

        // Primary must be set first before specifying a value for Secondary.
        if (!primaryIsSet && secondaryIsSet)
        {
            throw new ArgumentException($"{nameof(PrimaryProperty)} must be set first before specifying a value for {nameof(SecondaryProperty)}.");
        }

        // PrimaryBinding or CombinedBinding was set wtihout a corresponding value for Primary.
        if ((primaryBindingIsSet || combinedBindingIsSet) && !primaryIsSet)
        {
            throw new ArgumentException($"{nameof(PrimaryBindingProperty)} or {nameof(CombinedBindingProperty)} was set wtihout a corresponding value for {nameof(PrimaryProperty)}.");
        }

        // Primary is missing a trigger by an event or binding.
        if (primaryIsSet
            && !primaryBindingIsSet
            && !combinedBindingIsSet
            && primarySettings != null!
            && primarySettings.HasNoneEvent())
        {
            throw new ArgumentException($"{nameof(PrimaryProperty)} is missing a trigger by an event or binding.");
        }

        // SecondaryBinding was set wtihout a corresponding value for Secondary.
        if ((secondaryBindingIsSet || combinedBindingIsSet) && !secondaryIsSet)
        {
            throw new ArgumentException($"{nameof(SecondaryBindingProperty)} or {nameof(CombinedBindingProperty)} was set wtihout a corresponding value for {nameof(SecondaryProperty)}.");
        }

        // Secondary is missing a trigger by an event or binding.
        if (secondaryIsSet
            && !secondaryBindingIsSet
            && !combinedBindingIsSet
            && secondarySettings != null!
            && secondarySettings.HasNoneEvent())
        {
            throw new ArgumentException($"{nameof(SecondaryProperty)} is missing a trigger by an event or binding.");
        }

        // Cannot use StartWith without specifying a Primary animation.
        if (startWith != DependencyProperty.UnsetValue && !primaryIsSet)
        {
            throw new ArgumentException($"Cannot use {nameof(StartWithProperty)} without specifying a {nameof(PrimaryProperty)} animation.");
        }
    }

    internal static void ValidateListBox(ListBox element)
    {
        // Skip validation if a debugger isn't attached
        if (!Debugger.IsAttached)
        {
            return;
        }

        if (element is ListBox lb)
        {
            if (lb.ItemsSource == null)
            {
                Debug.WriteLine($"Cannot animate ListBox items because {nameof(lb.ItemsSource)} is null.");
                return;
            }

            var itemSettings = GetItems(lb);

            // ItemsProperty can only be set on a AnimatedListBox or AnimatedListView.
            if (itemSettings != null! && lb is not AnimatedListBox && lb is not AnimatedListView)
            {
                throw new ArgumentException($"{nameof(ItemsProperty)} can only be set on a {nameof(AnimatedListBox)} or {nameof(AnimatedListView)}.");
            }

            // ListBox item animations will only work if the inner ScrollViewer has CanContentScroll = true
            if (lb?.FindDescendant<ScrollViewer>() is ScrollViewer scroller && !scroller.CanContentScroll)
            {
                throw new ArgumentException($"{nameof(ListBox)} item animations will only work if the inner {nameof(ScrollViewer)} has {nameof(scroller.CanContentScroll)} = true");
            }

            // Don't set a value for the Event property, is it disregarded for ListBox item animations.
            if (itemSettings != null! && itemSettings.Event != DefaultSettings.Event)
            {
                throw new ArgumentException($"Don't set a value for the {nameof(itemSettings.Event)} property, is it disregarded for {nameof(ListBox)} item animations.");
            }
        }
    }
}
