namespace Lithium.Hytale.Math.Tests;

public class Vec2fTests
{
    [Fact]
    public void Add_TwoVectors_ReturnsCorrectSum()
    {
        var a = new Vec2f(1, 2);
        var b = new Vec2f(3, 4);

        var result = a + b;

        Assert.Equal(4, result.X);
        Assert.Equal(6, result.Y);
    }

    [Fact]
    public void Subtract_TwoVectors_ReturnsCorrectDifference()
    {
        var a = new Vec2f(5, 7);
        var b = new Vec2f(1, 2);

        var result = a - b;

        Assert.Equal(4, result.X);
        Assert.Equal(5, result.Y);
    }

    [Fact]
    public void Multiply_VectorByScalar_ReturnsScaledVector()
    {
        var v = new Vec2f(2, 3);

        var result = v * 2;

        Assert.Equal(4, result.X);
        Assert.Equal(6, result.Y);
    }

    [Fact]
    public void Dot_TwoVectors_ReturnsCorrectDotProduct()
    {
        var a = new Vec2f(1, 2);
        var b = new Vec2f(3, 4);

        var result = Vec2f.Dot(a, b);

        Assert.Equal(11, result); // 1*3 + 2*4 = 3 + 8 = 11
    }

    [Fact]
    public void Length_UnitVector_ReturnsOne()
    {
        var v = Vec2f.UnitX;

        var length = v.Length();

        Assert.Equal(1, length, precision: 5);
    }

    [Fact]
    public void Normalized_Returns_UnitLengthVector()
    {
        var v = new Vec2f(3, 4);

        var normalized = v.Normalized();

        Assert.Equal(1, normalized.Length(), precision: 5);
        Assert.Equal(0.6f, normalized.X, precision: 5);
        Assert.Equal(0.8f, normalized.Y, precision: 5);
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValues()
    {
        var original = new Vec2f(1.5f, 2.5f);
        var buffer = new byte[Vec2f.Size];

        original.Serialize(buffer);
        var deserialized = Vec2f.Deserialize(buffer);

        Assert.Equal(original, deserialized);
    }
}
