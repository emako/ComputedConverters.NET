using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ComputedAnimations.Extensions;
using static System.Windows.EventsMixin;
using FrameworkElement = System.Windows.FrameworkElement;
using Timeline = System.Windows.Media.Animation.Storyboard;

#pragma warning disable CS8631
#pragma warning disable CS8620

namespace ComputedAnimations;

public static partial class Animations
{
    private static readonly ConcurrentDictionary<Guid, ActiveTimeline<Timeline>> _actives = [];

    /// <summary>
    /// Function used to override the default animation values defined within XamlFlair
    /// </summary>
    public static void OverrideDefaultSettings(
        AnimationKind kind = DefaultSettings.DEFAULT_KIND,
        double duration = DefaultSettings.DEFAULT_DURATION,
        double interElementDelay = DefaultSettings.DEFAULT_INTER_ELEMENT_DELAY,
        EasingType easing = DefaultSettings.DEFAULT_EASING,
        EasingMode mode = DefaultSettings.DEFAULT_EASING_MODE,
        TransformationType transformOn = DefaultSettings.DEFAULT_TRANSFORM_ON,
        string @event = DefaultSettings.DEFAULT_EVENT)
    {
        DefaultSettings.Kind = kind;
        DefaultSettings.Duration = duration;
        DefaultSettings.InterElementDelay = interElementDelay;
        DefaultSettings.Easing = easing;
        DefaultSettings.Mode = mode;
        DefaultSettings.Event = @event;
        DefaultSettings.TransformOn = transformOn;
    }

    internal static bool IsInDesignMode(DependencyObject d)
    {
        return System.ComponentModel.DesignerProperties.GetIsInDesignMode(d);
    }

    private static void OnCombinedBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        HandleBindingChange(d, e, useSecondaryAnimation: false);
        HandleBindingChange(d, e, useSecondaryAnimation: true, invertAnimation: true);
    }

    private static void OnPrimaryBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        HandleBindingChange(d, e, useSecondaryAnimation: false);
    }

    private static void OnSecondaryBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        HandleBindingChange(d, e, useSecondaryAnimation: true);
    }

    private static void OnPrimaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Prevent running animations in a Visual Designer
        if (IsInDesignMode(d))
        {
            return;
        }

        if (d is FrameworkElement element)
        {
            InitializeElement(element);

            RegisterElementEvents(element, (e.NewValue as IAnimationSettings)!);
        }
    }

    private static void OnSecondaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Prevent running animations in a Visual Designer
        if (IsInDesignMode(d))
        {
            return;
        }

        if (d is FrameworkElement element)
        {
            InitializeElement(element);

            RegisterElementEvents(element, (e.NewValue as IAnimationSettings)!, useSecondarySettings: true);
        }
    }

    private static void OnStartWithChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Prevent running animations in a Visual Designer
        if (IsInDesignMode(d))
        {
            return;
        }

        if (d is FrameworkElement element)
        {
            InitializeElement(element);
        }
    }

    // This can be called from the three main entry-points (Primary, Secondary, and StartWith)
    private static void InitializeElement(FrameworkElement element)
    {
        if (Debugger.IsAttached && GetEnableDebugging(element) == DebugTarget.InitializeElement)
        {
            Debugger.Break();
        }

        if (GetIsInitialized(element))
        {
            return;
        }

        // Set IsInitialized to true to only run this code once per element
        SetIsInitialized(element, true);

        element
            .Events()
            .LoadedUntilUnloaded
            .Select(args => args.Sender as FrameworkElement)
            .Subscribe(
                elem =>
                {
                    // Perform validations on element's attached properties
                    Validate(elem!);

                    var startSettings = elem!.GetSettings(SettingsTarget.StartWith, getStartWithFunc: GetStartWith);

                    // If any StartWith settings were specified, apply them
                    if (startSettings != null)
                    {
                        elem!.ApplyInitialSettings((AnimationSettings)startSettings);
                    }
                },
                ex => Debug.WriteLine($"Error on subscription to the {nameof(FrameworkElementEvents.LoadedUntilUnloaded)} event.", ex)
            );

        element
            .Events()
            .Unloaded
            .Subscribe(
                args => CleanupDisposables((args.Sender as FrameworkElement)!),
                ex => Debug.WriteLine($"Error on subscription to the {nameof(FrameworkElement.Unloaded)} event.", ex)
            );

        element
            .Observe(UIElement.VisibilityProperty)
            .TakeUntil(element.Events().Unloaded)
            .Subscribe(
                _ =>
                {
                    var isVisible = element.Visibility == Visibility.Visible;
                    var elementGuid = GetElementGuid(element);

                    if (isVisible && _actives.GetNextIdleActiveTimeline(elementGuid)?.Timeline is Timeline idle)
                    {
                        RunNextAnimation(idle, element);
                    }
                },
                ex => Debug.WriteLine($"Error on subscription to the {nameof(FrameworkElement.Visibility)} changes of {nameof(FrameworkElement)}", ex)
            );
    }

    private static void RegisterElementEvents(FrameworkElement element, IAnimationSettings settings, bool useSecondarySettings = false)
    {
        var eventName = settings?.Event ?? nameof(FrameworkElement.Loaded);

        if (eventName.Equals(AnimationSettings.None, StringComparison.OrdinalIgnoreCase))
        {
            // Do nothing for "None"
        }
        else if (eventName.Equals(nameof(FrameworkElement.Visibility), StringComparison.OrdinalIgnoreCase))
        {
            element
                .Observe(FrameworkElement.VisibilityProperty)
                .Where(_ => element.Visibility == Visibility.Visible)
                .TakeUntil(element.Events().Unloaded)
                .Subscribe(
                    _ => PrepareAnimations(element, useSecondaryAnimation: useSecondarySettings),
                    ex => Debug.WriteLine($"Error on subscription to the {nameof(FrameworkElement.Visibility)} changes of {nameof(FrameworkElement)}", ex),
                    () => Cleanup(element)
                );
        }
        else if (eventName.Equals(nameof(FrameworkElement.DataContextChanged), StringComparison.OrdinalIgnoreCase))
        {
            element
                .Events()
                .DataContextChanged
                .DistinctUntilChanged(args => args.EventArgs.NewValue)
                .TakeUntil(element.Events().Unloaded)
                .Subscribe(
                    args => PrepareAnimations((args.Sender as FrameworkElement)!, useSecondaryAnimation: useSecondarySettings),
                    ex => Debug.WriteLine($"Error on subscription to the {nameof(FrameworkElement.DataContextChanged)} event.", ex),
                    () => Cleanup(element)
                );
        }
        else
        {
            Observable.FromEventPattern(element, eventName)
                .TakeUntil(element.Events().Unloaded)
                .Subscribe(
                    args => PrepareAnimations((args.Sender as FrameworkElement)!, useSecondaryAnimation: useSecondarySettings),
                    ex => Debug.WriteLine($"Error on subscription to the {eventName} event.", ex),
                    () => Cleanup(element));
        }
    }

    public static void OnAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            if (e.NewValue is AnimationSettings settings)
            {
                AnimationEventFetch.AddEventHandler(d, settings.Event, () => RunAnimation(element, settings));
            }
        }
    }

    private static void Timeline_Completed(object? sender, object e)
    {
        var timeline = (sender as ClockGroup)?.Timeline as Timeline;
        // Unregister the Completed event
        UnregisterTimeline(timeline!);

        if (_actives.GetElementByTimeline(timeline) is FrameworkElement element)
        {
            RunNextAnimation(timeline!, element);
        }
    }

    private static void RunNextAnimation(Timeline timeline, FrameworkElement element)
    {
        var timelineGuid = GetTimelineGuid(timeline);
        var elementGuid = GetElementGuid(element);
        var active = _actives.FindActiveTimeline(timelineGuid);

        active.SetAnimationState(timelineGuid, AnimationState.Completed);

        // If an idle animation exists, run it
        if (_actives.GetNextIdleActiveTimeline(elementGuid)?.Settings is AnimationSettings idleSettings)
        {
            RunAnimation(element, idleSettings, runFromIdle: true);
        }

        // Else if the animation is a repetitive sequence and they're all completed,
        // then reset the Completed to be Idle and re-start the sequence
        else if (active.IsSequence && active.IsIterating && _actives.AllIteratingCompleted(elementGuid))
        {
            _actives.ResetAllIteratingCompletedToIdle(elementGuid);

            // Make sure to run the next iteration on visible elements only
            if (element.Visibility == Visibility.Visible)
            {
                var first = _actives.FindFirstActiveTimeline(elementGuid);
                RunAnimation(element, first.Settings, runFromIdle: true);
            }
        }

        // Else if the animation needs to repeat, re-start it
        else if (!active.IsSequence && active.IsIterating)
        {
            active.SetAnimationState(timelineGuid, AnimationState.Idle);

            // Make sure to run the next iteration on visible elements only
            if (element.Visibility == Visibility.Visible)
            {
                RunAnimation(element, active.Settings, runFromIdle: true);
            }
        }

        // Else if it's done animating, clean it up
        else if (active.IterationCount <= 1 && active.IterationBehavior != IterationBehavior.Forever)
        {
            ExecuteCompletionCommand(element, active);
            Cleanup(elementGuid, stopAnimation: false);
        }
    }

    private static void HandleBindingChange(DependencyObject d, DependencyPropertyChangedEventArgs e, bool useSecondaryAnimation, bool invertAnimation = false)
    {
        // Prevent running animations in a Visual Designer
        if (IsInDesignMode(d))
        {
            return;
        }

        if (d is FrameworkElement element && e.NewValue is bool isAnimating)
        {
            // Handle regular animatons or "inverse" animations through the use of CombinedBinding
            if (isAnimating != invertAnimation)
            {
                PrepareAnimations(element, useSecondaryAnimation);
            }
            // Handle the case where a false value can be used to stop an iterating animation
            else if (!isAnimating)
            {
                StopIteratingAnimations(element);
            }
        }
    }

    public static void RunAnimation(FrameworkElement element, AnimationSettings settings, bool isSequence = false)
    {
        RunAnimation(element, settings, runFromIdle: false, isSequence: isSequence);
    }

    public static void RunAnimationFromSource(FrameworkElement element, string settingsSource, bool isSequence = false)
    {
        _ = element ?? throw new ArgumentNullException(nameof(element));

        object resource = element.Resources.FindName(settingsSource);

        if (resource is AnimationSettings settings)
        {
            RunAnimation(element, settings, isSequence);
        }
    }

    private static void RunAnimation(FrameworkElement element, AnimationSettings settings, bool runFromIdle, bool isSequence = false)
    {
        if (Debugger.IsAttached && GetEnableDebugging(element) == DebugTarget.RunAnimation)
        {
            Debugger.Break();
        }

        var timeline = new Timeline();
        var iterationBehavior = GetIterationBehavior(element);
        var iterationCount = GetIterationCount(element);

        // FADE IN/OUT
        if (settings.Kind.HasFlag(AnimationKind.FadeTo))
        {
            element.FadeTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.FadeFrom))
        {
            element.FadeFrom(settings, ref timeline);
        }

        // ROTATE TO/FROM
        if (settings.Kind.HasFlag(AnimationKind.RotateTo))
        {
            element.RotateTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.RotateFrom))
        {
            element.RotateFrom(settings, ref timeline);
        }

        // ColorAnimation supported only on Uno and WPF (not on native UWP due to Composition-only implementations)
        // COLOR TO/FROM
        if (settings.Kind.HasFlag(AnimationKind.ColorTo))
        {
            element.ColorTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.ColorFrom))
        {
            element.ColorFrom(settings, ref timeline);
        }

        // SCALE TO/FROM
        if (settings.Kind.HasFlag(AnimationKind.ScaleXTo))
        {
            element.ScaleXTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.ScaleXFrom))
        {
            element.ScaleXFrom(settings, ref timeline);
        }
        if (settings.Kind.HasFlag(AnimationKind.ScaleYTo))
        {
            element.ScaleYTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.ScaleYFrom))
        {
            element.ScaleYFrom(settings, ref timeline);
        }

        // TRANSLATE TO/FROM
        if (settings.Kind.HasFlag(AnimationKind.TranslateXTo))
        {
            element.TranslateXTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.TranslateXFrom))
        {
            element.TranslateXFrom(settings, ref timeline);
        }
        if (settings.Kind.HasFlag(AnimationKind.TranslateYTo))
        {
            element.TranslateYTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.TranslateYFrom))
        {
            element.TranslateYFrom(settings, ref timeline);
        }

        // BLUR TO/FROM
        if (settings.Kind.HasFlag(AnimationKind.BlurTo))
        {
            element.BlurTo(settings, ref timeline);
        }
        else if (settings.Kind.HasFlag(AnimationKind.BlurFrom))
        {
            element.BlurFrom(settings, ref timeline);
        }

        ActiveTimeline<Timeline> active = null!;

        if (runFromIdle)
        {
            // If the animation is running for an "idle" ActiveTimeline,
            // then it must be set to the existing ActiveTimeline
            // instead of creating a new one
            var guid = GetElementGuid(element);
            active = _actives.SetTimeline(guid, timeline);
        }
        else
        {
            // Add the new ActiveTimeline
            active = _actives.Add(timeline, settings, element, AnimationState.Idle, iterationBehavior, iterationCount, isSequence);
        }

        // We decrement the iteration count right before running the animation
        if (active.IterationCount > 0)
        {
            active.IterationCount--;
        }

        StartTimeline(timeline);
    }

    private static void StartTimeline(Timeline timeline)
    {
        timeline.Completed += Timeline_Completed;
        timeline.Begin();

        _actives.SetAnimationState(GetTimelineGuid(timeline), AnimationState.Running);
    }

    private static void PrepareAnimations(FrameworkElement element, bool useSecondaryAnimation = false)
    {
        if (element == null)
        {
            return;
        }

        // Make sure to not start an animation when an element is not visible
        if (element.Visibility != Visibility.Visible)
        {
            return;
        }

        // Make sure to stop any running animations
        if (_actives.IsElementAnimating(GetElementGuid(element)))
        {
            foreach (var active in _actives.GetAllNonIteratingActiveTimelines(GetElementGuid(element)))
            {
                active.Timeline.Stop();
            }
        }

        var animationSettings = element.GetSettings(
            useSecondaryAnimation ? SettingsTarget.Secondary : SettingsTarget.Primary,
            getPrimaryFunc: GetPrimary,
            getSecondaryFunc: GetSecondary);

        // Settings can be null if a Trigger is set before the associated element is loaded
        if (animationSettings == null)
        {
            return;
        }

        var settingsList = animationSettings.ToSettingsList();
        var startFirst = true;
        var iterationBehavior = GetIterationBehavior(element);
        var iterationCount = GetIterationCount(element);
        var sequenceCounter = 0;

        foreach (var settings in settingsList)
        {
            var isSequence = settingsList.Count > 1;

            // The "first" animation must always run immediately
            if (startFirst)
            {
                RunAnimation(element, settings, isSequence);

                startFirst = false;
            }
            else
            {
                _actives.Add(null, settings, element, AnimationState.Idle, iterationBehavior, iterationCount, isSequence, sequenceOrder: sequenceCounter);
            }

            sequenceCounter++;
        }
    }

    private static void StopIteratingAnimations(FrameworkElement element)
    {
        foreach (var active in _actives.GetAllIteratingActiveTimelines(GetElementGuid(element)))
        {
            active.IterationCount = 0;
            active.IterationBehavior = IterationBehavior.Count;
            _actives.RemoveByID(active.ElementGuid);
        }
    }

    private static void ExecuteCompletionCommand(FrameworkElement element, ActiveTimeline<Timeline> active)
    {
        // If a secondary completion command exists and the completing animation is a
        // secondary animation, execute the corresponding command
        if (GetSecondaryCompletionCommand(element) is ICommand secondaryCompletion
            && GetSecondary(element) is IAnimationSettings secondary
            && (AnimationSettings)secondary == active.Settings)
        {
            secondaryCompletion.Execute(null);
        }
        // Else execute the primary completion command if it exists
        else if (GetPrimaryCompletionCommand(element) is ICommand primaryCompletion)
        {
            primaryCompletion.Execute(null);
        }
    }

    private static void UnregisterTimeline(Timeline timeline)
    {
        if (timeline == null)
        {
            return;
        }
        var timelineGuid = GetTimelineGuid(timeline);

        // Retrieve the original Storyboard since the one passed in is
        // Frozen (to be able to unsubscribe from the event)
        var original = _actives.FindActiveTimeline(timelineGuid)?.Timeline;

        if (original != null)
        {
            original.Completed -= Timeline_Completed;
        }
    }

    private static void Cleanup(FrameworkElement element, bool includeIterating = true, bool stopAnimation = true)
    {
        Cleanup(GetElementGuid(element), stopAnimation);
    }

    private static void Cleanup(Guid elementGuid, bool stopAnimation = true)
    {
        var result = _actives.GetAllKeyValuePairs(elementGuid);

        foreach (var kvp in result.ToArray())
        {
            var timeline = kvp.Value.Timeline;

            if (timeline != null)
            {
                // We should only stop when the control unloads (since it can cause values to reset)
                if (stopAnimation)
                {
                    timeline?.Stop();
                }

                UnregisterTimeline(timeline!);
                CleanupTimeline(timeline!);
            }

            _actives.RemoveByID(kvp.Key);
        }
    }

    private static void CleanupTimeline(Timeline timeline)
    {
        if (timeline == null)
        {
            return;
        }

        var timelineGuid = GetTimelineGuid(timeline);
        // Retrieve the original Storyboard since the one passed in is
        // Frozen (to be able to unsubscribe from the event)
        var original = _actives.FindActiveTimeline(timelineGuid)?.Timeline;

        if (original != null)
        {
            original = null;
        }
        timeline = null!;
    }

    private static void CleanupDisposables(FrameworkElement element)
    {
        var disposables = GetDisposables(element);
        disposables?.Dispose();
        disposables = null;
    }
}
