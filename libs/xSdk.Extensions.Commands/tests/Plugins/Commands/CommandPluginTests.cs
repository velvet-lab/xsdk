using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Commands;

namespace xSdk.Extensions.Commands.Tests.Plugins.Commands;

public class CommandPluginTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void EnableCommands_CreatesCommandPlugin()
    {
        fixture.Builder
            .EnableCommands()
            .ConfigureServices((context, services) => services.AddPluginServices());

        var service = fixture.GetRequiredService<IPluginService>();
        var plugin = service.GetPlugin<CommandPlugin>();

        Assert.NotNull(plugin);
    }
}
