using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace ComputedAnimations.Extensions;

internal static class AnimationSettingsExtensions
{
    internal static EasingFunctionBase GetEase(this AnimationSettings settings)
    {
        EasingFunctionBase ease = settings.Easing switch
        {
            EasingType.Back => new BackEase(),
            EasingType.Bounce => new BounceEase(),
            EasingType.Circle => new CircleEase(),
            EasingType.Cubic => new CubicEase(),
            EasingType.Elastic => new ElasticEase(),
            EasingType.Linear => null!,
            EasingType.Quadratic => new QuadraticEase(),
            EasingType.Quartic => new QuarticEase(),
            EasingType.Quintic => new QuinticEase(),
            EasingType.Sine => new SineEase(),
            _ => new CubicEase(),
        };
        if (ease != null)
        {
            ease.EasingMode = settings.EasingMode;
        }

        return ease!;
    }

    internal static AnimationSettings ApplyOverrides(this AnimationSettings settings, AnimationSettings other)
    {
        var updated = new AnimationSettings();

        var kind = other.Kind;
        updated.Kind = kind != DefaultSettings.Kind ? kind : settings.Kind;

        var duration = other.Duration;
        updated.Duration = duration != DefaultSettings.Duration ? duration : settings.Duration;

        var delay = other.Delay;
        updated.Delay = delay != 0 ? delay : settings.Delay;

        var opacity = other.Opacity;
        updated.Opacity = opacity != 1 ? opacity : settings.Opacity;

        var offsetX = other.OffsetX;
        updated.OffsetX = offsetX != Offset.Empty ? offsetX : settings.OffsetX;

        var offsetY = other.OffsetY;
        updated.OffsetY = offsetY != Offset.Empty ? offsetY : settings.OffsetY;

        var rotation = other.Rotation;
        updated.Rotation = rotation != 0 ? rotation : settings.Rotation;

        // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
        var color = other.Color;
        updated.Color = color != DefaultSettings.Color ? color : settings.Color;

        var colorTarget = other.ColorOn;
        updated.ColorOn = colorTarget != DefaultSettings.ColorOn ? colorTarget : settings.ColorOn;
        // Blur not supported on Uno
        var blur = other.BlurRadius;
        updated.BlurRadius = blur != 0 ? blur : settings.BlurRadius;

        var scaleX = other.ScaleX;
        updated.ScaleX = scaleX != 1 ? scaleX : settings.ScaleX;

        var scaleY = other.ScaleY;
        updated.ScaleY = scaleY != 1 ? scaleY : settings.ScaleY;

        var origin = other.TransformCenterPoint;
        updated.TransformCenterPoint = origin != DefaultSettings.TransformCenterPoint ? origin : settings.TransformCenterPoint;

        var transformOn = other.TransformOn;
        updated.TransformOn = transformOn != DefaultSettings.TransformOn ? transformOn : settings.TransformOn;

        var easingMode = other.EasingMode;
        updated.EasingMode = easingMode != DefaultSettings.Mode ? easingMode : settings.EasingMode;

        var easing = other.Easing;
        updated.Easing = easing != DefaultSettings.Easing ? easing : settings.Easing;

        var @event = other.Event;
        updated.Event = @event != DefaultSettings.Event ? @event : settings.Event;

        return updated;
    }

    internal static List<AnimationSettings> ToSettingsList(this IAnimationSettings settings)
    {
        var animations = new List<AnimationSettings>();

        if (settings is CompoundSettings compound)
        {
            animations.AddRange(compound.Sequence);
        }
        else if (settings is AnimationSettings anim)
        {
            animations.Add(anim);
        }

        return animations;
    }
}
