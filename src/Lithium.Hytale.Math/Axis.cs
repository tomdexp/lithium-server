using System.Runtime.CompilerServices;

namespace Lithium.Hytale.Math;

/// <summary>
/// Represents a 3D axis (X, Y, or Z).
/// </summary>
public enum Axis : byte
{
    X = 0,
    Y = 1,
    Z = 2
}

/// <summary>
/// Extension methods for the Axis enum.
/// Optimized with switch expressions and precomputed lookups.
/// </summary>
public static class AxisExtensions
{
    private static readonly Vec3f[] s_directions =
    [
        Vec3f.UnitX, // X
        Vec3f.UnitY, // Y
        Vec3f.UnitZ  // Z
    ];

    private static readonly Vec3f[] s_negativeDirections =
    [
        new Vec3f(-1, 0, 0), // -X
        new Vec3f(0, -1, 0), // -Y
        new Vec3f(0, 0, -1)  // -Z
    ];

    /// <summary>
    /// Gets the unit direction vector for this axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f GetDirection(this Axis axis) => s_directions[(int)axis];

    /// <summary>
    /// Gets the negative unit direction vector for this axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f GetNegativeDirection(this Axis axis) => s_negativeDirections[(int)axis];

    /// <summary>
    /// Rotates the axis 90 degrees around the given rotation axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Axis RotateAround(this Axis axis, Axis rotationAxis) => (axis, rotationAxis) switch
    {
        (Axis.X, Axis.Y) => Axis.Z,
        (Axis.X, Axis.Z) => Axis.Y,
        (Axis.Y, Axis.X) => Axis.Z,
        (Axis.Y, Axis.Z) => Axis.X,
        (Axis.Z, Axis.X) => Axis.Y,
        (Axis.Z, Axis.Y) => Axis.X,
        _ => axis // Rotating around self returns self
    };

    /// <summary>
    /// Returns the next axis in cyclic order (X -> Y -> Z -> X).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Axis Next(this Axis axis) => axis switch
    {
        Axis.X => Axis.Y,
        Axis.Y => Axis.Z,
        Axis.Z => Axis.X,
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };

    /// <summary>
    /// Returns the previous axis in cyclic order (X -> Z -> Y -> X).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Axis Previous(this Axis axis) => axis switch
    {
        Axis.X => Axis.Z,
        Axis.Y => Axis.X,
        Axis.Z => Axis.Y,
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };

    /// <summary>
    /// Gets the component value of a Vec3f along this axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetComponent(this Axis axis, Vec3f v) => axis switch
    {
        Axis.X => v.X,
        Axis.Y => v.Y,
        Axis.Z => v.Z,
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };

    /// <summary>
    /// Sets the component value of a Vec3f along this axis and returns the modified vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3f SetComponent(this Axis axis, Vec3f v, float value) => axis switch
    {
        Axis.X => new Vec3f(value, v.Y, v.Z),
        Axis.Y => new Vec3f(v.X, value, v.Z),
        Axis.Z => new Vec3f(v.X, v.Y, value),
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };
}
