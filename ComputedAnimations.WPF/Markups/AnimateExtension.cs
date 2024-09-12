using ComputedAnimations.Extensions;
using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ComputedAnimations;

[MarkupExtensionReturnType(typeof(IAnimationSettings))]
public class AnimateExtension : MarkupExtension
{
    /// <summary>
    /// Specifies the base settings to retrieve initial values from
    /// </summary>
    public IAnimationSettings BasedOn { get; set; } = null!;

    /// <summary>
    /// Specifies the animation types to include in the composite animation
    /// </summary>
    public AnimationKind Kind { get; set; } = DefaultSettings.Kind;

    /// <summary>
    /// Specifies the duration of the composite animation
    /// </summary>
    public double Duration { get; set; } = DefaultSettings.Duration;

    /// <summary>
    /// Specifies the delay of the composite animation
    /// </summary>
    public double Delay { get; set; }

    /// <summary>
    /// Specifies the target opacity of the composite animation
    /// </summary>
    public double Opacity { get; set; } = 1;

    /// <summary>
    /// Specifies the target x-offset of the composite animation
    /// </summary>
    /// <remarks>
    /// OffsetX must be a double or a star-based value (ex: 150 or 0.75*)
    /// </remarks>
    public Offset OffsetX { get; set; } = Offset.Empty;

    /// <summary>
    /// Specifies the target y-offset of the composite animation
    /// </summary>
    /// <remarks>
    /// OffsetY must be a double or a star-based value (ex: 150 or 0.75*)
    /// </remarks>
    public Offset OffsetY { get; set; } = Offset.Empty;

    /// <summary>
    /// Specifies the target x-scaling of the composite animation
    /// </summary>
    public double ScaleX { get; set; } = 1;

    /// <summary>
    /// Specifies the target y-scaling of the composite animation
    /// </summary>
    public double ScaleY { get; set; } = 1;

    /// <summary>
    /// Specifies the target rotation (in degrees) of the composite animation
    /// </summary>
    public double Rotation { get; set; }

    // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
    /// <summary>
    /// Specifies the target color of the composite animation
    /// </summary>
    public Color Color { get; set; } = DefaultSettings.Color;

    /// <summary>
    /// Specifies the target property for a color animation
    /// </summary>
    public ColorTarget ColorOn { get; set; } = DefaultSettings.ColorOn;

    // Blur not supported on Uno

    /// <summary>
    /// Specifies the blur amount of the composite animation
    /// </summary>
    public double BlurRadius { get; set; }

    /// <summary>
    /// Specifies the center point of the element's transform
    /// </summary>
    public Point TransformCenterPoint { get; set; } = DefaultSettings.TransformCenterPoint;

    /// <summary>
    /// Specifies the transformation type to use (render or layout)
    /// </summary>
    public TransformationType TransformOn { get; set; } = DefaultSettings.TransformOn;

    /// <summary>
    /// Specifies the event used to trigger the composite animation
    /// </summary>
    /// <remarks>
    /// This property is disregarded for controls based on ListViewBase (UWP) or ListBox (WPF)
    /// </remarks>
    public string Event { get; set; } = DefaultSettings.Event;

    /// <summary>
    /// Specifies the easing of the composite animation
    /// </summary>
    public EasingType Easing { get; set; } = DefaultSettings.Easing;

    /// <summary>
    /// Specifies the easing's mode of the composite animation
    /// </summary>
    public EasingMode EasingMode { get; set; } = DefaultSettings.Mode;

    private IAnimationSettings GetAnimationSettings()
    {
        if (BasedOn is CompoundSettings compound)
        {
            // Make sure to capture an override on the Event property (if any)
            if (Event != DefaultSettings.Event)
            {
                compound.Event = Event;
            }

            return compound;
        }

        var current = new AnimationSettings()
        {
            Kind = Kind,
            Duration = Duration,
            Delay = Delay,
            Opacity = Opacity,
            OffsetX = OffsetX,
            OffsetY = OffsetY,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            Rotation = Rotation,
            // Blur not supported on Uno
            BlurRadius = BlurRadius,
            TransformCenterPoint = TransformCenterPoint,
            Easing = Easing,
            EasingMode = EasingMode,
            Event = Event,
            TransformOn = TransformOn,
            // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
            Color = Color,
            ColorOn = ColorOn,
        };

        // If "BasedOn" is used, return an AnimationSettings
        // object that uses the values in "BasedOn" and then
        // overrides those with updated values from "current"
        return BasedOn == null
            ? current
            : ((AnimationSettings)BasedOn)?.ApplyOverrides(current)!;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => GetAnimationSettings();
}
