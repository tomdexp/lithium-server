using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// A high-performance 4D vector using single-precision floats.
/// Optimized with SIMD operations via System.Numerics.Vector4.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vec4f : IEquatable<Vec4f>
{
    public const int Size = 16;

    public float X;
    public float Y;
    public float Z;
    public float W;

    public static readonly Vec4f Zero = new(0, 0, 0, 0);
    public static readonly Vec4f One = new(1, 1, 1, 1);
    public static readonly Vec4f UnitX = new(1, 0, 0, 0);
    public static readonly Vec4f UnitY = new(0, 1, 0, 0);
    public static readonly Vec4f UnitZ = new(0, 0, 1, 0);
    public static readonly Vec4f UnitW = new(0, 0, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec4f(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec4f(Vector4 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        W = v.W;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec4f(Vec3f xyz, float w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    /// <summary>
    /// Converts to System.Numerics.Vector4 for SIMD operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector4 AsVector4() => new(X, Y, Z, W);

    /// <summary>
    /// Returns the XYZ components as a Vec3f.
    /// </summary>
    public readonly Vec3f XYZ => new(X, Y, Z);

    // SIMD-optimized operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator +(Vec4f a, Vec4f b) => new(a.AsVector4() + b.AsVector4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator -(Vec4f a, Vec4f b) => new(a.AsVector4() - b.AsVector4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator *(Vec4f a, Vec4f b) => new(a.AsVector4() * b.AsVector4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator /(Vec4f a, Vec4f b) => new(a.AsVector4() / b.AsVector4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator *(Vec4f a, float scalar) => new(a.AsVector4() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator *(float scalar, Vec4f a) => new(a.AsVector4() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator /(Vec4f a, float scalar) => new(a.AsVector4() / scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f operator -(Vec4f a) => new(-a.X, -a.Y, -a.Z, -a.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec4f left, Vec4f right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec4f left, Vec4f right) => !left.Equals(right);

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vec4f a, Vec4f b) => Vector4.Dot(a.AsVector4(), b.AsVector4());

    /// <summary>
    /// Returns the length (magnitude) of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length() => AsVector4().Length();

    /// <summary>
    /// Returns the squared length of the vector (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float LengthSquared() => AsVector4().LengthSquared();

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec4f a, Vec4f b) => Vector4.Distance(a.AsVector4(), b.AsVector4());

    /// <summary>
    /// Returns the normalized (unit length) vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec4f Normalized() => new(Vector4.Normalize(AsVector4()));

    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f Lerp(Vec4f a, Vec4f b, float t) => new(Vector4.Lerp(a.AsVector4(), b.AsVector4(), t));

    /// <summary>
    /// Returns the component-wise minimum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f Min(Vec4f a, Vec4f b) => new(Vector4.Min(a.AsVector4(), b.AsVector4()));

    /// <summary>
    /// Returns the component-wise maximum of two vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f Max(Vec4f a, Vec4f b) => new(Vector4.Max(a.AsVector4(), b.AsVector4()));

    /// <summary>
    /// Clamps each component between the corresponding values in min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f Clamp(Vec4f value, Vec4f min, Vec4f max)
        => new(Vector4.Clamp(value.AsVector4(), min.AsVector4(), max.AsVector4()));

    // Serialization
    /// <summary>
    /// Deserializes a Vec4f from a ReadOnlySpan of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4f Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
            throw new ArgumentException($"Data must be at least {Size} bytes.", nameof(data));

        return MemoryMarshal.Read<Vec4f>(data);
    }

    /// <summary>
    /// Serializes this Vec4f to a Span of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Serialize(Span<byte> destination)
    {
        if (destination.Length < Size)
            throw new ArgumentException($"Destination must be at least {Size} bytes.", nameof(destination));

        MemoryMarshal.Write(destination, in this);
    }

    public readonly bool Equals(Vec4f other)
        => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    public override readonly bool Equals(object? obj)
        => obj is Vec4f other && Equals(other);

    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y, Z, W);

    public override readonly string ToString()
        => $"Vec4f({X}, {Y}, {Z}, {W})";
}
