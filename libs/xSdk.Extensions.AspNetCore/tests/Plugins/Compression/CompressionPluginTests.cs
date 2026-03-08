using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;

namespace xSdk.Plugins.Compression;

public class CompressionPluginTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void CreatePlugin()
    {
        fixture.Builder
            .EnableCompression()
            .ConfigureServices((context, services) => services.AddPluginServices());

        var service = fixture.GetRequiredService<IPluginService>();
        var plugin = service.GetPlugin<CompressionPlugin>();

        Assert.NotNull(plugin);
    }
}
