using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using System.Windows;

namespace ComputedAnimations;

#pragma warning disable CS0660
#pragma warning disable CS0661
#pragma warning disable CS8767

[SuppressMessage("", "CS0660:Unable to override Object.Equals since DependencyObject is sealed.", Justification = "<Pending>")]
[SuppressMessage("", "CS0661:Unable to override Object.GetHashCode since DependencyObject is sealed.", Justification = "<Pending>")]
public class CompoundSettings : DependencyObject, IAnimationSettings, IEqualityComparer<CompoundSettings>
{
    public string Event
    {
        get => (string)GetValue(EventProperty);
        set => SetValue(EventProperty, value);
    }

    /// <summary>
    /// Specifies the event used to trigger the composite animation
    /// </summary>
    public static readonly DependencyProperty EventProperty =
        DependencyProperty.Register(
            nameof(Event),
            typeof(string),
            typeof(CompoundSettings),
            new PropertyMetadata(DefaultSettings.Event));

    ///// <summary>
    ///// Specifies the list of AnimationSettings used for a compound animation
    ///// </summary>
    public List<AnimationSettings> Sequence { get; set; } = new List<AnimationSettings>();

    public bool Equals(CompoundSettings other)
    {
        if (object.ReferenceEquals(null, other)) return false;  // Is null?
        if (object.ReferenceEquals(this, other)) return true;   // Is the same object?

        return IsEqual(other);
    }

    private bool IsEqual(CompoundSettings obj)
    {
        return obj is CompoundSettings other
            && other.Event.Equals(Event)
            && other.Sequence.SequenceEqual(Sequence);
    }

    public bool Equals(CompoundSettings x, CompoundSettings y)
    {
        if (object.ReferenceEquals(null, y)) return false;    // Is null?
        if (object.ReferenceEquals(x, y)) return true;        // Is the same object?
        if (x.GetType() != y.GetType()) return false;        // Is the same type?

        return IsEqual((CompoundSettings)y);
    }

    public int GetHashCode(CompoundSettings obj)
        => InternalGetHashCode();

    public new bool Equals(object obj)
    {
        if (object.ReferenceEquals(null, obj)) return false;    // Is null?
        if (object.ReferenceEquals(this, obj)) return true;     // Is the same object?
        if (obj.GetType() != this.GetType()) return false;      // Is the same type?

        return IsEqual((CompoundSettings)obj);
    }

    public new int GetHashCode()
        => InternalGetHashCode();

    private int InternalGetHashCode()
    {
        unchecked
        {
            // Choose large primes to avoid hashing collisions
            const int HashingBase = (int)2166136261;
            const int HashingMultiplier = 16777619;

            int hash = HashingBase;
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Event) ? Event.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Sequence) ? Sequence.GetHashCode() : 0);
            return hash;
        }
    }

    public static bool operator ==(CompoundSettings obj, CompoundSettings other)
    {
        if (object.ReferenceEquals(obj, other)) return true;
        if (object.ReferenceEquals(null, obj)) return false;    // Ensure that "obj" isn't null

        return obj.Equals(other);
    }

    public static bool operator !=(CompoundSettings obj, CompoundSettings other) => !(obj == other);
}
