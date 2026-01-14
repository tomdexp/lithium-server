using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// A high-performance 4x4 matrix using single-precision floats.
/// Wraps System.Numerics.Matrix4x4 for SIMD-optimized operations.
/// Layout: Row-major (M11, M12, M13, M14, M21, ..., M44).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Mat4f : IEquatable<Mat4f>
{
    public float M11, M12, M13, M14;
    public float M21, M22, M23, M24;
    public float M31, M32, M33, M34;
    public float M41, M42, M43, M44;

    public static readonly Mat4f Identity = new(Matrix4x4.Identity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mat4f(
        float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        M11 = m11; M12 = m12; M13 = m13; M14 = m14;
        M21 = m21; M22 = m22; M23 = m23; M24 = m24;
        M31 = m31; M32 = m32; M33 = m33; M34 = m34;
        M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mat4f(Matrix4x4 m)
    {
        M11 = m.M11; M12 = m.M12; M13 = m.M13; M14 = m.M14;
        M21 = m.M21; M22 = m.M22; M23 = m.M23; M24 = m.M24;
        M31 = m.M31; M32 = m.M32; M33 = m.M33; M34 = m.M34;
        M41 = m.M41; M42 = m.M42; M43 = m.M43; M44 = m.M44;
    }

    /// <summary>
    /// Converts to System.Numerics.Matrix4x4 for SIMD operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Matrix4x4 AsMatrix4x4() => new(
        M11, M12, M13, M14,
        M21, M22, M23, M24,
        M31, M32, M33, M34,
        M41, M42, M43, M44);

    // SIMD-optimized operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f operator +(Mat4f a, Mat4f b) => new(a.AsMatrix4x4() + b.AsMatrix4x4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f operator -(Mat4f a, Mat4f b) => new(a.AsMatrix4x4() - b.AsMatrix4x4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f operator *(Mat4f a, Mat4f b) => new(a.AsMatrix4x4() * b.AsMatrix4x4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f operator *(Mat4f a, float scalar) => new(a.AsMatrix4x4() * scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f operator -(Mat4f a) => new(-a.AsMatrix4x4());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Mat4f left, Mat4f right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Mat4f left, Mat4f right) => !left.Equals(right);

    /// <summary>
    /// Transforms a Vec3f by this matrix (assumes w=1 for position).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec3f Transform(Vec3f v)
        => new(Vector3.Transform(v.AsVector3(), AsMatrix4x4()));

    /// <summary>
    /// Transforms a direction vector (ignores translation).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec3f TransformNormal(Vec3f v)
        => new(Vector3.TransformNormal(v.AsVector3(), AsMatrix4x4()));

    /// <summary>
    /// Returns the determinant of this matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float GetDeterminant() => AsMatrix4x4().GetDeterminant();

    /// <summary>
    /// Attempts to invert the matrix. Returns false if singular.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryInvert(out Mat4f result)
    {
        if (Matrix4x4.Invert(AsMatrix4x4(), out var inverted))
        {
            result = new Mat4f(inverted);
            return true;
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Returns the transposed matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Mat4f Transpose() => new(Matrix4x4.Transpose(AsMatrix4x4()));

    // Factory methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateTranslation(Vec3f position)
        => new(Matrix4x4.CreateTranslation(position.AsVector3()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateScale(Vec3f scale)
        => new(Matrix4x4.CreateScale(scale.AsVector3()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateScale(float scale)
        => new(Matrix4x4.CreateScale(scale));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateRotationX(float radians)
        => new(Matrix4x4.CreateRotationX(radians));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateRotationY(float radians)
        => new(Matrix4x4.CreateRotationY(radians));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateRotationZ(float radians)
        => new(Matrix4x4.CreateRotationZ(radians));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateLookAt(Vec3f cameraPosition, Vec3f cameraTarget, Vec3f cameraUpVector)
        => new(Matrix4x4.CreateLookAt(cameraPosition.AsVector3(), cameraTarget.AsVector3(), cameraUpVector.AsVector3()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        => new(Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
        => new(Matrix4x4.CreateOrthographic(width, height, zNearPlane, zFarPlane));

    // Serialization
    /// <summary>
    /// Deserializes a Mat4f from a ReadOnlySpan of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat4f Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length < 64)
            throw new ArgumentException("Data must be at least 64 bytes.", nameof(data));

        return MemoryMarshal.Read<Mat4f>(data);
    }

    /// <summary>
    /// Serializes this Mat4f to a Span of bytes (Little Endian).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Serialize(Span<byte> destination)
    {
        if (destination.Length < 64)
            throw new ArgumentException("Destination must be at least 64 bytes.", nameof(destination));

        MemoryMarshal.Write(destination, in this);
    }

    public readonly bool Equals(Mat4f other)
        => AsMatrix4x4().Equals(other.AsMatrix4x4());

    public override readonly bool Equals(object? obj)
        => obj is Mat4f other && Equals(other);

    public override readonly int GetHashCode()
        => AsMatrix4x4().GetHashCode();

    public override readonly string ToString()
        => AsMatrix4x4().ToString();
}
