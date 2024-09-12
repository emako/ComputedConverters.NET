// ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)

namespace ComputedAnimations;

public enum ColorTarget
{
    Background = 0,
    Foreground,
    BorderBrush,
    Fill,
    Stroke,
}
