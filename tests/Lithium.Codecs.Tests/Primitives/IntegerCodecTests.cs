using System.Buffers;
using Lithium.Codecs.Primitives;

namespace Lithium.Codecs.Tests.Primitives;

public sealed class IntegerCodecTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void IntegerCodec_Should_EncodeAndDecode(int value)
    {
        // Arrange
        var codec = new IntegerCodec();
        var bufferWriter = new ArrayBufferWriter<byte>();

        // Act
        codec.Encode(value, bufferWriter);
        
        var sequence = new ReadOnlySequence<byte>(bufferWriter.WrittenMemory);
        var reader = new SequenceReader<byte>(sequence);
        var decoded = codec.Decode(ref reader);

        // Assert
        Assert.Equal(value, decoded);
    }
}