namespace Lithium.Hytale.Math.Tests;

public class Vec3fTests
{
    [Fact]
    public void Add_TwoVectors_ReturnsCorrectSum()
    {
        var a = new Vec3f(1, 2, 3);
        var b = new Vec3f(4, 5, 6);

        var result = a + b;

        Assert.Equal(5, result.X);
        Assert.Equal(7, result.Y);
        Assert.Equal(9, result.Z);
    }

    [Fact]
    public void Subtract_TwoVectors_ReturnsCorrectDifference()
    {
        var a = new Vec3f(5, 7, 9);
        var b = new Vec3f(1, 2, 3);

        var result = a - b;

        Assert.Equal(4, result.X);
        Assert.Equal(5, result.Y);
        Assert.Equal(6, result.Z);
    }

    [Fact]
    public void Multiply_VectorByScalar_ReturnsScaledVector()
    {
        var v = new Vec3f(1, 2, 3);

        var result = v * 2;

        Assert.Equal(2, result.X);
        Assert.Equal(4, result.Y);
        Assert.Equal(6, result.Z);
    }

    [Fact]
    public void Dot_TwoVectors_ReturnsCorrectDotProduct()
    {
        var a = new Vec3f(1, 2, 3);
        var b = new Vec3f(4, 5, 6);

        var result = Vec3f.Dot(a, b);

        Assert.Equal(32, result); // 1*4 + 2*5 + 3*6 = 4 + 10 + 18 = 32
    }

    [Fact]
    public void Cross_TwoVectors_ReturnsCorrectCrossProduct()
    {
        var a = new Vec3f(1, 0, 0);
        var b = new Vec3f(0, 1, 0);

        var result = Vec3f.Cross(a, b);

        Assert.Equal(0, result.X);
        Assert.Equal(0, result.Y);
        Assert.Equal(1, result.Z);
    }

    [Fact]
    public void Length_UnitVector_ReturnsOne()
    {
        var v = Vec3f.UnitX;

        var length = v.Length();

        Assert.Equal(1, length, precision: 5);
    }

    [Fact]
    public void Normalized_Returns_UnitLengthVector()
    {
        var v = new Vec3f(3, 4, 0);

        var normalized = v.Normalized();

        Assert.Equal(1, normalized.Length(), precision: 5);
        Assert.Equal(0.6f, normalized.X, precision: 5);
        Assert.Equal(0.8f, normalized.Y, precision: 5);
    }

    [Fact]
    public void Distance_TwoVectors_ReturnsCorrectDistance()
    {
        var a = new Vec3f(0, 0, 0);
        var b = new Vec3f(3, 4, 0);

        var distance = Vec3f.Distance(a, b);

        Assert.Equal(5, distance, precision: 5);
    }

    [Fact]
    public void Lerp_HalfwayBetweenTwoVectors()
    {
        var a = Vec3f.Zero;
        var b = new Vec3f(10, 10, 10);

        var result = Vec3f.Lerp(a, b, 0.5f);

        Assert.Equal(5, result.X);
        Assert.Equal(5, result.Y);
        Assert.Equal(5, result.Z);
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValues()
    {
        var original = new Vec3f(1.5f, 2.5f, 3.5f);
        var buffer = new byte[12];

        original.Serialize(buffer);
        var deserialized = Vec3f.Deserialize(buffer);

        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = new Vec3f(1, 2, 3);
        var b = new Vec3f(1, 2, 3);

        Assert.True(a == b);
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = new Vec3f(1, 2, 3);
        var b = new Vec3f(4, 5, 6);

        Assert.False(a == b);
        Assert.True(a != b);
    }
}
