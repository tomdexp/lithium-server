using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// A high-performance 3D vector using single-precision floats.
/// Optimized with SIMD operations via System.Numerics.Vector3.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vec3f : IEquatable<Vec3f>
{
    public float X;
    public float Y;
    public float Z;

    public static readonly Vec3f Zero = new(0, 0, 0);
    public static readonly Vec3f One = new(1, 1, 1);
    public static readonly Vec3f UnitX = new(1, 0, 0);
    public static readonly Vec3f UnitY = new(0, 1, 0);
    public static readonly Vec3f UnitZ = new(0, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3f(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3f(Vector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    /// <summary>
    /// Converts to System.Numerics.Vector3 for SIMD operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector3 AsVector3() => new(X, Y, Z);

    // SIMD-optimized operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator +(Vec3f a, Vec3f b) => new(a.AsVector3() + b.AsVector3());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator -(Vec3f a, Vec3f b) => new(a.AsVector3() - b.AsVector3());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator *(Vec3f a, Vec3f b) => new(a.AsVector3() * b.AsVector3());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator /(Vec3f a, Vec3f b) => new(a.AsVector3() / b.AsVector3());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator *(Vec3f a, float scalar) => new(a.AsVector3() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator *(float scalar, Vec3f a) => new(a.AsVector3() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator /(Vec3f a, float scalar) => new(a.AsVector3() / scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f operator -(Vec3f a) => new(-a.X, -a.Y, -a.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec3f left, Vec3f right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec3f left, Vec3f right) => !left.Equals(right);

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vec3f a, Vec3f b) => Vector3.Dot(a.AsVector3(), b.AsVector3());

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Cross(Vec3f a, Vec3f b) => new(Vector3.Cross(a.AsVector3(), b.AsVector3()));

    /// <summary>
    /// Returns the length (magnitude) of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length() => AsVector3().Length();

    /// <summary>
    /// Returns the squared length of the vector (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float LengthSquared() => AsVector3().LengthSquared();

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec3f a, Vec3f b) => Vector3.Distance(a.AsVector3(), b.AsVector3());

    /// <summary>
    /// Returns the squared distance between two vectors (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vec3f a, Vec3f b) => Vector3.DistanceSquared(a.AsVector3(), b.AsVector3());

    /// <summary>
    /// Returns the normalized (unit length) vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec3f Normalized() => new(Vector3.Normalize(AsVector3()));

    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Lerp(Vec3f a, Vec3f b, float t) => new(Vector3.Lerp(a.AsVector3(), b.AsVector3(), t));

    /// <summary>
    /// Returns the component-wise minimum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Min(Vec3f a, Vec3f b) => new(Vector3.Min(a.AsVector3(), b.AsVector3()));

    /// <summary>
    /// Returns the component-wise maximum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Max(Vec3f a, Vec3f b) => new(Vector3.Max(a.AsVector3(), b.AsVector3()));

    /// <summary>
    /// Clamps each component between the corresponding values in min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Clamp(Vec3f value, Vec3f min, Vec3f max)
        => new(Vector3.Clamp(value.AsVector3(), min.AsVector3(), max.AsVector3()));

    // Serialization
    /// <summary>
    /// Deserializes a Vec3f from a ReadOnlySpan of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length < 12)
            throw new ArgumentException("Data must be at least 12 bytes.", nameof(data));

        return MemoryMarshal.Read<Vec3f>(data);
    }

    /// <summary>
    /// Serializes this Vec3f to a Span of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Serialize(Span<byte> destination)
    {
        if (destination.Length < 12)
            throw new ArgumentException("Destination must be at least 12 bytes.", nameof(destination));

        MemoryMarshal.Write(destination, in this);
    }

    public readonly bool Equals(Vec3f other)
        => X == other.X && Y == other.Y && Z == other.Z;

    public override readonly bool Equals(object? obj)
        => obj is Vec3f other && Equals(other);

    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y, Z);

    public override readonly string ToString()
        => $"Vec3f({X}, {Y}, {Z})";
}
