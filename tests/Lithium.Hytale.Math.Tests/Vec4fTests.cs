namespace Lithium.Hytale.Math.Tests;

public class Vec4fTests
{
    [Fact]
    public void Add_TwoVectors_ReturnsCorrectSum()
    {
        var a = new Vec4f(1, 2, 3, 4);
        var b = new Vec4f(5, 6, 7, 8);

        var result = a + b;

        Assert.Equal(6, result.X);
        Assert.Equal(8, result.Y);
        Assert.Equal(10, result.Z);
        Assert.Equal(12, result.W);
    }

    [Fact]
    public void Subtract_TwoVectors_ReturnsCorrectDifference()
    {
        var a = new Vec4f(5, 7, 9, 11);
        var b = new Vec4f(1, 2, 3, 4);

        var result = a - b;

        Assert.Equal(4, result.X);
        Assert.Equal(5, result.Y);
        Assert.Equal(6, result.Z);
        Assert.Equal(7, result.W);
    }

    [Fact]
    public void Multiply_VectorByScalar_ReturnsScaledVector()
    {
        var v = new Vec4f(1, 2, 3, 4);

        var result = v * 2;

        Assert.Equal(2, result.X);
        Assert.Equal(4, result.Y);
        Assert.Equal(6, result.Z);
        Assert.Equal(8, result.W);
    }

    [Fact]
    public void Dot_TwoVectors_ReturnsCorrectDotProduct()
    {
        var a = new Vec4f(1, 2, 3, 4);
        var b = new Vec4f(5, 6, 7, 8);

        var result = Vec4f.Dot(a, b);

        Assert.Equal(70, result); // 1*5 + 2*6 + 3*7 + 4*8 = 5 + 12 + 21 + 32 = 70
    }

    [Fact]
    public void XYZ_ReturnsVec3fWithCorrectValues()
    {
        var v = new Vec4f(1, 2, 3, 4);

        var xyz = v.XYZ;

        Assert.Equal(1, xyz.X);
        Assert.Equal(2, xyz.Y);
        Assert.Equal(3, xyz.Z);
    }

    [Fact]
    public void Constructor_FromVec3fAndW_SetsCorrectValues()
    {
        var xyz = new Vec3f(1, 2, 3);

        var v = new Vec4f(xyz, 4);

        Assert.Equal(1, v.X);
        Assert.Equal(2, v.Y);
        Assert.Equal(3, v.Z);
        Assert.Equal(4, v.W);
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValues()
    {
        var original = new Vec4f(1.5f, 2.5f, 3.5f, 4.5f);
        var buffer = new byte[Vec4f.Size];

        original.Serialize(buffer);
        var deserialized = Vec4f.Deserialize(buffer);

        Assert.Equal(original, deserialized);
    }
}
