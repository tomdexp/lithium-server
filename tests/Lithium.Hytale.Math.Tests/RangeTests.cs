namespace Lithium.Hytale.Math.Tests;

public class RangeTests
{
    [Fact]
    public void Contains_ValueInRange_ReturnsTrue()
    {
        var range = new Range<int>(0, 10);

        Assert.True(range.Contains(5));
        Assert.True(range.Contains(0));  // Min is inclusive
        Assert.True(range.Contains(10)); // Max is inclusive
    }

    [Fact]
    public void Contains_ValueOutOfRange_ReturnsFalse()
    {
        var range = new Range<int>(0, 10);

        Assert.False(range.Contains(-1));
        Assert.False(range.Contains(11));
    }

    [Fact]
    public void Overlaps_OverlappingRanges_ReturnsTrue()
    {
        var a = new Range<int>(0, 10);
        var b = new Range<int>(5, 15);

        Assert.True(a.Overlaps(b));
        Assert.True(b.Overlaps(a));
    }

    [Fact]
    public void Overlaps_AdjacentRanges_ReturnsTrue()
    {
        var a = new Range<int>(0, 5);
        var b = new Range<int>(5, 10);

        // Adjacent at boundary should overlap (inclusive)
        Assert.True(a.Overlaps(b));
    }

    [Fact]
    public void Overlaps_NonOverlappingRanges_ReturnsFalse()
    {
        var a = new Range<int>(0, 4);
        var b = new Range<int>(6, 10);

        Assert.False(a.Overlaps(b));
        Assert.False(b.Overlaps(a));
    }

    [Fact]
    public void Intersect_OverlappingRanges_ReturnsIntersection()
    {
        var a = new Range<int>(0, 10);
        var b = new Range<int>(5, 15);

        var intersection = a.Intersect(b);

        Assert.NotNull(intersection);
        Assert.Equal(5, intersection.Value.Min);
        Assert.Equal(10, intersection.Value.Max);
    }

    [Fact]
    public void Intersect_NonOverlappingRanges_ReturnsNull()
    {
        var a = new Range<int>(0, 4);
        var b = new Range<int>(6, 10);

        var intersection = a.Intersect(b);

        Assert.Null(intersection);
    }

    [Fact]
    public void Constructor_MinGreaterThanMax_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Range<int>(10, 5));
    }

    [Fact]
    public void Equals_SameRanges_ReturnsTrue()
    {
        var a = new Range<float>(0.5f, 1.5f);
        var b = new Range<float>(0.5f, 1.5f);

        Assert.True(a == b);
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_DifferentRanges_ReturnsFalse()
    {
        var a = new Range<float>(0.5f, 1.5f);
        var b = new Range<float>(0.5f, 2.0f);

        Assert.False(a == b);
        Assert.True(a != b);
    }
}
