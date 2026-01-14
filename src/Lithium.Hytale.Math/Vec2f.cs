using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// A high-performance 2D vector using single-precision floats.
/// Optimized with SIMD operations via System.Numerics.Vector2.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vec2f : IEquatable<Vec2f>
{
    public const int Size = 8;

    public float X;
    public float Y;

    public static readonly Vec2f Zero = new(0, 0);
    public static readonly Vec2f One = new(1, 1);
    public static readonly Vec2f UnitX = new(1, 0);
    public static readonly Vec2f UnitY = new(0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec2f(float x, float y)
    {
        X = x;
        Y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec2f(Vector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    /// <summary>
    /// Converts to System.Numerics.Vector2 for SIMD operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector2 AsVector2() => new(X, Y);

    // SIMD-optimized operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator +(Vec2f a, Vec2f b) => new(a.AsVector2() + b.AsVector2());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator -(Vec2f a, Vec2f b) => new(a.AsVector2() - b.AsVector2());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator *(Vec2f a, Vec2f b) => new(a.AsVector2() * b.AsVector2());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator /(Vec2f a, Vec2f b) => new(a.AsVector2() / b.AsVector2());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator *(Vec2f a, float scalar) => new(a.AsVector2() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator *(float scalar, Vec2f a) => new(a.AsVector2() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator /(Vec2f a, float scalar) => new(a.AsVector2() / scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f operator -(Vec2f a) => new(-a.X, -a.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec2f left, Vec2f right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec2f left, Vec2f right) => !left.Equals(right);

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vec2f a, Vec2f b) => Vector2.Dot(a.AsVector2(), b.AsVector2());

    /// <summary>
    /// Returns the length (magnitude) of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length() => AsVector2().Length();

    /// <summary>
    /// Returns the squared length of the vector (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float LengthSquared() => AsVector2().LengthSquared();

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec2f a, Vec2f b) => Vector2.Distance(a.AsVector2(), b.AsVector2());

    /// <summary>
    /// Returns the squared distance between two vectors (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vec2f a, Vec2f b) => Vector2.DistanceSquared(a.AsVector2(), b.AsVector2());

    /// <summary>
    /// Returns the normalized (unit length) vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec2f Normalized() => new(Vector2.Normalize(AsVector2()));

    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f Lerp(Vec2f a, Vec2f b, float t) => new(Vector2.Lerp(a.AsVector2(), b.AsVector2(), t));

    /// <summary>
    /// Returns the component-wise minimum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f Min(Vec2f a, Vec2f b) => new(Vector2.Min(a.AsVector2(), b.AsVector2()));

    /// <summary>
    /// Returns the component-wise maximum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f Max(Vec2f a, Vec2f b) => new(Vector2.Max(a.AsVector2(), b.AsVector2()));

    /// <summary>
    /// Clamps each component between the corresponding values in min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f Clamp(Vec2f value, Vec2f min, Vec2f max)
        => new(Vector2.Clamp(value.AsVector2(), min.AsVector2(), max.AsVector2()));

    // Serialization
    /// <summary>
    /// Deserializes a Vec2f from a ReadOnlySpan of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2f Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
            throw new ArgumentException($"Data must be at least {Size} bytes.", nameof(data));

        return MemoryMarshal.Read<Vec2f>(data);
    }

    /// <summary>
    /// Serializes this Vec2f to a Span of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Serialize(Span<byte> destination)
    {
        if (destination.Length < Size)
            throw new ArgumentException($"Destination must be at least {Size} bytes.", nameof(destination));

        MemoryMarshal.Write(destination, in this);
    }

    public readonly bool Equals(Vec2f other)
        => X == other.X && Y == other.Y;

    public override readonly bool Equals(object? obj)
        => obj is Vec2f other && Equals(other);

    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y);

    public override readonly string ToString()
        => $"Vec2f({X}, {Y})";
}
