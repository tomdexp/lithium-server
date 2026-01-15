using Lithium.Codecs.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace Lithium.Codecs.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddLithiumCodecs_Should_RegisterAllPrimitiveCodecs()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLithiumCodecs();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var registry = serviceProvider.GetService<ICodecRegistry>();

        // Assert
        Assert.NotNull(registry);
        
        Assert.IsType<IntegerCodec>(registry.Get<int>());
    }
}
