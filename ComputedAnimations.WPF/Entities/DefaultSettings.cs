using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ComputedAnimations;

internal static class DefaultSettings
{
    internal const AnimationKind DEFAULT_KIND = AnimationKind.FadeTo;
    internal const double DEFAULT_DURATION = 500;
    internal const double DEFAULT_INTER_ELEMENT_DELAY = 25;
    internal static readonly Point DEFAULT_TRANSFORM_CENTER_POINT = new Point(0.5, 0.5);
    internal const EasingType DEFAULT_EASING = EasingType.Cubic;
    internal const EasingMode DEFAULT_EASING_MODE = EasingMode.EaseOut;
    internal const string DEFAULT_EVENT = nameof(FrameworkElement.Loaded);

    // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
    internal static readonly Color DEFAULT_COLOR = Colors.Transparent;

    internal static readonly ColorTarget DEFAULT_COLOR_ON = ColorTarget.Background;

    internal const TransformationType DEFAULT_TRANSFORM_ON = TransformationType.Render;

    internal static AnimationKind Kind { get; set; } = DEFAULT_KIND;
    internal static double Duration { get; set; } = DEFAULT_DURATION;
    internal static double InterElementDelay { get; set; } = DEFAULT_INTER_ELEMENT_DELAY;
    internal static Point TransformCenterPoint { get; set; } = DEFAULT_TRANSFORM_CENTER_POINT;
    internal static EasingType Easing { get; set; } = DEFAULT_EASING;
    internal static EasingMode Mode { get; set; } = DEFAULT_EASING_MODE;
    internal static string Event { get; set; } = DEFAULT_EVENT;

    // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
    internal static Color Color { get; set; } = DEFAULT_COLOR;

    internal static ColorTarget ColorOn { get; set; } = DEFAULT_COLOR_ON;

    internal static TransformationType TransformOn { get; set; } = DEFAULT_TRANSFORM_ON;
}
