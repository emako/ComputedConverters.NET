using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace ComputedAnimations;

#pragma warning disable CS0660
#pragma warning disable CS0661
#pragma warning disable CA2013

[SuppressMessage("", "CS0660:Unable to override Object.Equals since DependencyObject is sealed.", Justification = "<Pending>")]
[SuppressMessage("", "CS0661:Unable to override Object.GetHashCode since DependencyObject is sealed.", Justification = "<Pending>")]
internal partial class ActiveTimeline<T> : IEquatable<ActiveTimeline<T>> where T : DependencyObject
{
    internal Guid ElementGuid { get; set; } = default;

    internal T Timeline { get; set; } = null!;

    internal AnimationSettings Settings { get; set; } = null!;

    internal FrameworkElement Element { get; set; } = null!;

    internal AnimationState State { get; set; } = default;

    internal IterationBehavior IterationBehavior { get; set; } = default;

    internal int IterationCount { get; set; }

    internal bool IsSequence { get; set; }

    internal int SequenceOrder { get; set; }

    internal bool IsIterating { get => IterationCount > 0 || IterationBehavior == IterationBehavior.Forever; }

    public new bool Equals(object obj)
    {
        if (object.ReferenceEquals(null, obj)) return false;    // Is null?
        if (object.ReferenceEquals(this, obj)) return true;     // Is the same object?
        if (obj.GetType() != this.GetType()) return false;      // Is the same type?

        return IsEqual((ActiveTimeline<T>)obj);
    }

    public bool Equals(ActiveTimeline<T>? other)
    {
        if (object.ReferenceEquals(null, other)) return false;  // Is null?
        if (object.ReferenceEquals(this, other)) return true;   // Is the same object?

        return IsEqual(other);
    }

    private bool IsEqual(ActiveTimeline<T> obj)
    {
        return obj is ActiveTimeline<T> other
            && other.ElementGuid.Equals(ElementGuid)
            && other.Timeline.Equals(Timeline)
            && other.Settings.Equals(Settings)
            && other.Settings.Equals(Element)
            && other.Settings.Equals(State)
            && other.Settings.Equals(IterationBehavior)
            && other.Settings.Equals(IterationCount)
            && other.Settings.Equals(IsSequence)
            && other.Settings.Equals(SequenceOrder)
            && other.Settings.Equals(IsIterating);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            // Choose large primes to avoid hashing collisions
            const int HashingBase = (int)2166136261;
            const int HashingMultiplier = 16777619;

            int hash = HashingBase;
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, ElementGuid) ? ElementGuid.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Timeline) ? Timeline.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Settings) ? Settings.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Element) ? Element.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, State) ? State.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, IterationBehavior) ? IterationBehavior.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, IterationCount) ? IterationCount.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, IsSequence) ? IsSequence.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, SequenceOrder) ? SequenceOrder.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, IsIterating) ? IsIterating.GetHashCode() : 0);
            return hash;
        }
    }

    public static bool operator ==(ActiveTimeline<T> obj, ActiveTimeline<T> other)
    {
        if (object.ReferenceEquals(obj, other)) return true;
        if (object.ReferenceEquals(null, obj)) return false;    // Ensure that "obj" isn't null

        return obj.Equals(other);
    }

    public static bool operator !=(ActiveTimeline<T> obj, ActiveTimeline<T> other) => !(obj == other);
}
