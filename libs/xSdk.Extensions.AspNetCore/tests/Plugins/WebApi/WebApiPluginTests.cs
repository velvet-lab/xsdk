using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;

namespace xSdk.Plugins.WebApi;

public class WebApiPluginTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void CreatePlugin()
    {
        fixture.Builder
            .EnableWebApi()
            .ConfigureServices((context, services) => services.AddPluginServices());

        var service = fixture.GetRequiredService<IPluginService>();
        var plugin = service.GetPlugin<WebApiPlugin>();

        Assert.NotNull(plugin);
    }
}
