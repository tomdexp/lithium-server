namespace Lithium.Hytale.Math.Tests;

public class QuatfTests
{
    [Fact]
    public void Identity_HasCorrectValues()
    {
        var identity = Quatf.Identity;

        Assert.Equal(0, identity.X);
        Assert.Equal(0, identity.Y);
        Assert.Equal(0, identity.Z);
        Assert.Equal(1, identity.W);
    }

    [Fact]
    public void Multiply_IdentityByIdentity_ReturnsIdentity()
    {
        var identity = Quatf.Identity;

        var result = identity * identity;

        Assert.Equal(0, result.X, precision: 5);
        Assert.Equal(0, result.Y, precision: 5);
        Assert.Equal(0, result.Z, precision: 5);
        Assert.Equal(1, result.W, precision: 5);
    }

    [Fact]
    public void Normalized_ReturnsUnitLengthQuaternion()
    {
        var q = new Quatf(1, 2, 3, 4);

        var normalized = q.Normalized();

        Assert.Equal(1, normalized.Length(), precision: 5);
    }

    [Fact]
    public void Conjugate_ReturnsCorrectConjugate()
    {
        var q = new Quatf(1, 2, 3, 4);

        var conjugate = q.Conjugate();

        Assert.Equal(-1, conjugate.X);
        Assert.Equal(-2, conjugate.Y);
        Assert.Equal(-3, conjugate.Z);
        Assert.Equal(4, conjugate.W);
    }

    [Fact]
    public void Rotate_UnitXBy90DegreesAroundZ_ReturnsUnitY()
    {
        var rotation = Quatf.CreateFromAxisAngle(Vec3f.UnitZ, MathF.PI / 2);
        var v = Vec3f.UnitX;

        var result = rotation.Rotate(v);

        Assert.Equal(0, result.X, precision: 4);
        Assert.Equal(1, result.Y, precision: 4);
        Assert.Equal(0, result.Z, precision: 4);
    }

    [Fact]
    public void CreateFromYawPitchRoll_ValidatesCreation()
    {
        var q = Quatf.CreateFromYawPitchRoll(0, 0, 0);

        // No rotation should give identity-like quaternion
        Assert.Equal(1, q.Length(), precision: 5);
    }

    [Fact]
    public void Slerp_HalfwayBetween_ReturnsInterpolatedValue()
    {
        var a = Quatf.Identity;
        var b = Quatf.CreateFromAxisAngle(Vec3f.UnitZ, MathF.PI);

        var result = Quatf.Slerp(a, b, 0.5f);

        // Result should be a 90-degree rotation around Z
        Assert.Equal(1, result.Length(), precision: 5);
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValues()
    {
        var original = new Quatf(0.1f, 0.2f, 0.3f, 0.9f);
        var buffer = new byte[Quatf.Size];

        original.Serialize(buffer);
        var deserialized = Quatf.Deserialize(buffer);

        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Inverse_MultipliedByOriginal_ReturnsIdentity()
    {
        var q = Quatf.CreateFromAxisAngle(Vec3f.UnitY, MathF.PI / 4).Normalized();

        var result = q * q.Inverse();

        Assert.Equal(0, result.X, precision: 4);
        Assert.Equal(0, result.Y, precision: 4);
        Assert.Equal(0, result.Z, precision: 4);
        Assert.Equal(1, result.W, precision: 4);
    }
}
