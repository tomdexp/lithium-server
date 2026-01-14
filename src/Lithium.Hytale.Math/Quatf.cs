using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// A high-performance quaternion using single-precision floats.
/// Optimized with System.Numerics.Quaternion for SIMD operations.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Quatf : IEquatable<Quatf>
{
    public const int Size = 16;

    public float X;
    public float Y;
    public float Z;
    public float W;

    public static readonly Quatf Identity = new(0, 0, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quatf(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quatf(Quaternion q)
    {
        X = q.X;
        Y = q.Y;
        Z = q.Z;
        W = q.W;
    }

    /// <summary>
    /// Converts to System.Numerics.Quaternion for SIMD operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Quaternion AsQuaternion() => new(X, Y, Z, W);

    // Operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf operator *(Quatf a, Quatf b) => new(a.AsQuaternion() * b.AsQuaternion());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf operator -(Quatf a) => new(-a.X, -a.Y, -a.Z, -a.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quatf left, Quatf right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quatf left, Quatf right) => !left.Equals(right);

    /// <summary>
    /// Computes the dot product of two quaternions.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Quatf a, Quatf b) => Quaternion.Dot(a.AsQuaternion(), b.AsQuaternion());

    /// <summary>
    /// Returns the length (magnitude) of the quaternion.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length() => AsQuaternion().Length();

    /// <summary>
    /// Returns the squared length of the quaternion (avoids sqrt).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float LengthSquared() => AsQuaternion().LengthSquared();

    /// <summary>
    /// Returns the normalized quaternion.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Quatf Normalized() => new(Quaternion.Normalize(AsQuaternion()));

    /// <summary>
    /// Returns the conjugate of the quaternion.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Quatf Conjugate() => new(Quaternion.Conjugate(AsQuaternion()));

    /// <summary>
    /// Returns the inverse of the quaternion.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Quatf Inverse() => new(Quaternion.Inverse(AsQuaternion()));

    /// <summary>
    /// Spherically interpolates between two quaternions.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf Slerp(Quatf a, Quatf b, float t) => new(Quaternion.Slerp(a.AsQuaternion(), b.AsQuaternion(), t));

    /// <summary>
    /// Linearly interpolates between two quaternions and normalizes the result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf Lerp(Quatf a, Quatf b, float t) => new(Quaternion.Lerp(a.AsQuaternion(), b.AsQuaternion(), t));

    /// <summary>
    /// Rotates a vector by this quaternion.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec3f Rotate(Vec3f v) => new(Vector3.Transform(v.AsVector3(), AsQuaternion()));

    // Factory methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf CreateFromAxisAngle(Vec3f axis, float angle)
        => new(Quaternion.CreateFromAxisAngle(axis.AsVector3(), angle));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        => new(Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf CreateFromRotationMatrix(Mat4f matrix)
        => new(Quaternion.CreateFromRotationMatrix(matrix.AsMatrix4x4()));

    // Serialization
    /// <summary>
    /// Deserializes a Quatf from a ReadOnlySpan of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quatf Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length < Size)
            throw new ArgumentException($"Data must be at least {Size} bytes.", nameof(data));

        return MemoryMarshal.Read<Quatf>(data);
    }

    /// <summary>
    /// Serializes this Quatf to a Span of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Serialize(Span<byte> destination)
    {
        if (destination.Length < Size)
            throw new ArgumentException($"Destination must be at least {Size} bytes.", nameof(destination));

        MemoryMarshal.Write(destination, in this);
    }

    public readonly bool Equals(Quatf other)
        => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    public override readonly bool Equals(object? obj)
        => obj is Quatf other && Equals(other);

    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y, Z, W);

    public override readonly string ToString()
        => $"Quatf({X}, {Y}, {Z}, {W})";
}
