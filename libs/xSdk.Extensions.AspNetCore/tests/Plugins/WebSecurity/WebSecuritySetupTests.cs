using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;

namespace xSdk.Plugins.WebSecurity;

public class WebSecuritySetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void WebSecuritySetup_DefaultProperties_AreEmpty()
    {
        var setup = new WebSecuritySetup();

        Assert.NotNull(setup);
        Assert.True(string.IsNullOrEmpty(setup.Origins));
    }

    [Fact]
    public void WebSecurityPlugin_CreatedViaHostBuilder()
    {
        fixture.Builder
            .EnableWebSecurity()
            .ConfigureServices((context, services) => services.AddPluginServices());

        var service = fixture.GetRequiredService<IPluginService>();
        var plugin = service.GetPlugin<WebSecurityPlugin>();

        Assert.NotNull(plugin);
    }

    [Fact]
    public void WebSecuritySetup_Definitions_OriginsName_IsCorrect()
    {
        Assert.Equal("origins", WebSecuritySetup.Definitions.Origins.Name);
    }

    [Fact]
    public void WebSecuritySetup_Definitions_OriginsTemplate_IsCorrect()
    {
        Assert.Contains("origins", WebSecuritySetup.Definitions.Origins.Template);
    }
}
