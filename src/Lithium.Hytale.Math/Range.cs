namespace Lithium.Hytale.Math;

/// <summary>
/// A generic read-only range with inclusive min and max values.
/// </summary>
/// <typeparam name="T">The type of the range bounds.</typeparam>
public readonly struct Range<T> : IEquatable<Range<T>>
    where T : IComparable<T>
{
    /// <summary>
    /// The minimum (inclusive) value of the range.
    /// </summary>
    public T Min { get; }

    /// <summary>
    /// The maximum (inclusive) value of the range.
    /// </summary>
    public T Max { get; }

    /// <summary>
    /// Creates a new range with the specified min and max values.
    /// </summary>
    public Range(T min, T max)
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException("Min must be less than or equal to Max.");

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Checks if a value is within this range (inclusive).
    /// </summary>
    public bool Contains(T value)
        => value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;

    /// <summary>
    /// Checks if this range overlaps with another range.
    /// </summary>
    public bool Overlaps(Range<T> other)
        => Min.CompareTo(other.Max) <= 0 && Max.CompareTo(other.Min) >= 0;

    /// <summary>
    /// Returns the intersection of this range with another, or null if they don't overlap.
    /// </summary>
    public Range<T>? Intersect(Range<T> other)
    {
        if (!Overlaps(other))
            return null;

        var newMin = Min.CompareTo(other.Min) > 0 ? Min : other.Min;
        var newMax = Max.CompareTo(other.Max) < 0 ? Max : other.Max;
        return new Range<T>(newMin, newMax);
    }

    public bool Equals(Range<T> other)
        => EqualityComparer<T>.Default.Equals(Min, other.Min)
           && EqualityComparer<T>.Default.Equals(Max, other.Max);

    public override bool Equals(object? obj)
        => obj is Range<T> other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Min, Max);

    public static bool operator ==(Range<T> left, Range<T> right) => left.Equals(right);

    public static bool operator !=(Range<T> left, Range<T> right) => !left.Equals(right);

    public override string ToString()
        => $"Range[{Min}, {Max}]";
}
