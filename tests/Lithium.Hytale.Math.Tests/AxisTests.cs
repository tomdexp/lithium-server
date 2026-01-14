namespace Lithium.Hytale.Math.Tests;

public class AxisTests
{
    [Theory]
    [InlineData(Axis.X, 1, 0, 0)]
    [InlineData(Axis.Y, 0, 1, 0)]
    [InlineData(Axis.Z, 0, 0, 1)]
    public void GetDirection_ReturnsCorrectUnitVector(Axis axis, float expectedX, float expectedY, float expectedZ)
    {
        var direction = axis.GetDirection();

        Assert.Equal(expectedX, direction.X);
        Assert.Equal(expectedY, direction.Y);
        Assert.Equal(expectedZ, direction.Z);
    }

    [Theory]
    [InlineData(Axis.X, -1, 0, 0)]
    [InlineData(Axis.Y, 0, -1, 0)]
    [InlineData(Axis.Z, 0, 0, -1)]
    public void GetNegativeDirection_ReturnsCorrectNegativeUnitVector(Axis axis, float expectedX, float expectedY, float expectedZ)
    {
        var direction = axis.GetNegativeDirection();

        Assert.Equal(expectedX, direction.X);
        Assert.Equal(expectedY, direction.Y);
        Assert.Equal(expectedZ, direction.Z);
    }

    [Theory]
    [InlineData(Axis.X, Axis.Y)]
    [InlineData(Axis.Y, Axis.Z)]
    [InlineData(Axis.Z, Axis.X)]
    public void Next_ReturnsCyclicNextAxis(Axis input, Axis expected)
    {
        Assert.Equal(expected, input.Next());
    }

    [Theory]
    [InlineData(Axis.X, Axis.Z)]
    [InlineData(Axis.Y, Axis.X)]
    [InlineData(Axis.Z, Axis.Y)]
    public void Previous_ReturnsCyclicPreviousAxis(Axis input, Axis expected)
    {
        Assert.Equal(expected, input.Previous());
    }

    [Fact]
    public void GetComponent_ReturnsCorrectComponent()
    {
        var v = new Vec3f(1, 2, 3);

        Assert.Equal(1, Axis.X.GetComponent(v));
        Assert.Equal(2, Axis.Y.GetComponent(v));
        Assert.Equal(3, Axis.Z.GetComponent(v));
    }

    [Fact]
    public void SetComponent_SetsCorrectComponent()
    {
        var v = new Vec3f(1, 2, 3);

        var resultX = Axis.X.SetComponent(v, 10);
        var resultY = Axis.Y.SetComponent(v, 20);
        var resultZ = Axis.Z.SetComponent(v, 30);

        Assert.Equal(new Vec3f(10, 2, 3), resultX);
        Assert.Equal(new Vec3f(1, 20, 3), resultY);
        Assert.Equal(new Vec3f(1, 2, 30), resultZ);
    }

    [Theory]
    [InlineData(Axis.X, Axis.Y, Axis.Z)]
    [InlineData(Axis.X, Axis.Z, Axis.Y)]
    [InlineData(Axis.Y, Axis.X, Axis.Z)]
    [InlineData(Axis.Y, Axis.Z, Axis.X)]
    [InlineData(Axis.Z, Axis.X, Axis.Y)]
    [InlineData(Axis.Z, Axis.Y, Axis.X)]
    [InlineData(Axis.X, Axis.X, Axis.X)] // Rotating around self
    public void RotateAround_ReturnsExpectedAxis(Axis axis, Axis rotationAxis, Axis expected)
    {
        Assert.Equal(expected, axis.RotateAround(rotationAxis));
    }
}
