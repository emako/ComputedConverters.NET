using System;
using System.Windows;

namespace ComputedAnimations.Extensions;

internal static class FrameworkElementExtensions
{
    internal static IAnimationSettings GetSettings(
        this FrameworkElement element,
        SettingsTarget target,
        Func<FrameworkElement, IAnimationSettings> getPrimaryFunc = null!,
        Func<FrameworkElement, IAnimationSettings> getSecondaryFunc = null!,
        Func<FrameworkElement, AnimationSettings> getStartWithFunc = null!)
    {
        IAnimationSettings settings = null!;

        switch (target)
        {
            case SettingsTarget.Primary:
                settings = getPrimaryFunc(element);
                break;

            case SettingsTarget.Secondary:
                settings = getSecondaryFunc(element);
                break;

            case SettingsTarget.StartWith:
                settings = getStartWithFunc(element);
                break;
        }

        // Settings can be null if a Trigger is set before the associated element is loaded
        if (settings == null)
        {
            return null!;
        }

        return settings;
    }
}
