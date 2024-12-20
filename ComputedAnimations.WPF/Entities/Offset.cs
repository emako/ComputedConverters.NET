﻿using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;

#pragma warning disable CA2013
#pragma warning disable CS0660
#pragma warning disable CS0661

namespace ComputedAnimations;

[SuppressMessage("", "CS0660:Unable to override Object.Equals since DependencyObject is sealed.", Justification = "<Pending>")]
[SuppressMessage("", "CS0661:Unable to override Object.GetHashCode since DependencyObject is sealed.", Justification = "<Pending>")]
[TypeConverter(typeof(OffsetTypeConverter))]
public class Offset
{
    internal static Offset Empty = new();

    public double OffsetFactor { get; set; }

    public double OffsetValue { get; set; }

    internal OffsetTarget Target { get; set; }

    public static Offset ConvertToOffsetFactor(string translation)
    {
        return Offset.Create(translation.Trim());
    }

    internal double GetCalculatedOffset(FrameworkElement element, OffsetTarget target)
    {
        // Make sure that an offset value is used
        // if an offset factor wasn't specified
        if (OffsetFactor == 0 && (OffsetValue > 0 || OffsetValue < 0))
        {
            return OffsetValue;
        }

        var width = element.ActualWidth > 0
            ? element.ActualWidth
            : element.Width > 0
                ? element.Width
                : 0;

        var height = element.ActualHeight > 0
            ? element.ActualHeight
            : element.Height > 0
                ? element.Height
                : 0;

        return target == OffsetTarget.X
            ? width * OffsetFactor
            : height * OffsetFactor;
    }

    internal static Offset Create(string offsetValue)
    {
        if (offsetValue.Equals("*", StringComparison.InvariantCulture))
        {
            return new Offset()
            {
                OffsetFactor = 1.0
            };
        }
        else if (offsetValue.EndsWith("*") && double.TryParse(offsetValue.TrimEnd('*'), NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
        {
            return new Offset()
            {
                OffsetFactor = result
            };
        }
        else if (double.TryParse(offsetValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var dbl))
        {
            return new Offset()
            {
                OffsetValue = dbl
            };
        }

        throw new ArgumentException($"{nameof(Offset)} must be a double or a star-based value (ex: 150 or 0.75*).");
    }

    public override string ToString()
    {
        if (OffsetValue > 0 || OffsetValue < 0)
        {
            return OffsetValue.ToString();
        }

        return OffsetFactor.ToString();
    }

    public bool Equals(Offset other)
    {
        if (object.ReferenceEquals(null, other)) return false;  // Is null?
        if (object.ReferenceEquals(this, other)) return true;   // Is the same object?

        return IsEqual(other);
    }

    private bool IsEqual(Offset obj)
    {
        return obj is Offset other
            && other.OffsetValue.Equals(OffsetValue)
            && other.OffsetFactor.Equals(OffsetFactor)
            && other.Target.Equals(Target);
    }

    public bool Equals(Offset x, Offset y)
    {
        if (object.ReferenceEquals(null, y)) return false;  // Is null?
        if (object.ReferenceEquals(x, y)) return true;      // Is the same object?
        if (x.GetType() != y.GetType()) return false;       // Is the same type?

        return IsEqual((Offset)y);
    }

    public int GetHashCode(Offset obj)
        => InternalGetHashCode();

    public new bool Equals(object obj)
    {
        if (object.ReferenceEquals(null, obj)) return false;    // Is null?
        if (object.ReferenceEquals(this, obj)) return true;     // Is the same object?
        if (obj.GetType() != this.GetType()) return false;      // Is the same type?

        return IsEqual((Offset)obj);
    }

    public override int GetHashCode()
        => InternalGetHashCode();

    private int InternalGetHashCode()
    {
        unchecked
        {
            // Choose large primes to avoid hashing collisions
            const int HashingBase = (int)2166136261;
            const int HashingMultiplier = 16777619;

            int hash = HashingBase;
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, OffsetValue) ? OffsetValue.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, OffsetFactor) ? OffsetFactor.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Target) ? Target.GetHashCode() : 0);
            return hash;
        }
    }

    public static bool operator ==(Offset obj, Offset other)
    {
        if (object.ReferenceEquals(obj, other)) return true;
        if (object.ReferenceEquals(null, obj)) return false;    // Ensure that "obj" isn't null

        return obj.Equals(other);
    }

    public static bool operator !=(Offset obj, Offset other) => !(obj == other);
}

public class OffsetTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        var offsetValue = ((value as string) ?? string.Empty).Trim();

        return Offset.Create(offsetValue);
    }
}
