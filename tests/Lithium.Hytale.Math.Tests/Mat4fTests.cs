namespace Lithium.Hytale.Math.Tests;

public class Mat4fTests
{
    [Fact]
    public void Identity_MultipliedByIdentity_ReturnsIdentity()
    {
        var identity = Mat4f.Identity;

        var result = identity * identity;

        Assert.Equal(Mat4f.Identity, result);
    }

    [Fact]
    public void Multiply_TwoMatrices_ReturnsCorrectProduct()
    {
        var a = Mat4f.CreateTranslation(new Vec3f(1, 0, 0));
        var b = Mat4f.CreateTranslation(new Vec3f(0, 1, 0));

        var result = a * b;

        // Combined translation should be (1, 1, 0)
        Assert.Equal(1, result.M41, precision: 5);
        Assert.Equal(1, result.M42, precision: 5);
        Assert.Equal(0, result.M43, precision: 5);
    }

    [Fact]
    public void Transform_TranslatesPosition()
    {
        var translation = Mat4f.CreateTranslation(new Vec3f(10, 20, 30));
        var position = Vec3f.Zero;

        var result = translation.Transform(position);

        Assert.Equal(10, result.X, precision: 5);
        Assert.Equal(20, result.Y, precision: 5);
        Assert.Equal(30, result.Z, precision: 5);
    }

    [Fact]
    public void Transform_ScalesPosition()
    {
        var scale = Mat4f.CreateScale(2);
        var position = new Vec3f(1, 2, 3);

        var result = scale.Transform(position);

        Assert.Equal(2, result.X, precision: 5);
        Assert.Equal(4, result.Y, precision: 5);
        Assert.Equal(6, result.Z, precision: 5);
    }

    [Fact]
    public void Transpose_CorrectlySwapsRowsAndColumns()
    {
        var m = new Mat4f(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16);

        var t = m.Transpose();

        Assert.Equal(1, t.M11);
        Assert.Equal(5, t.M12);
        Assert.Equal(9, t.M13);
        Assert.Equal(13, t.M14);
        Assert.Equal(2, t.M21);
    }

    [Fact]
    public void TryInvert_Identity_ReturnsIdentity()
    {
        var identity = Mat4f.Identity;

        var success = identity.TryInvert(out var result);

        Assert.True(success);
        Assert.Equal(Mat4f.Identity, result);
    }

    [Fact]
    public void GetDeterminant_Identity_ReturnsOne()
    {
        var identity = Mat4f.Identity;

        var det = identity.GetDeterminant();

        Assert.Equal(1, det, precision: 5);
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValues()
    {
        var original = Mat4f.CreateTranslation(new Vec3f(1, 2, 3));
        var buffer = new byte[64];

        original.Serialize(buffer);
        var deserialized = Mat4f.Deserialize(buffer);

        Assert.Equal(original, deserialized);
    }
}
