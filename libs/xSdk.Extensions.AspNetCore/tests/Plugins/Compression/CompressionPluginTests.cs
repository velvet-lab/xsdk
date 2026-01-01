using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Plugins.Compression
{
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
}
