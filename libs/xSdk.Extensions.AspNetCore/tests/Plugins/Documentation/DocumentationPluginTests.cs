using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Documentation;

public class DocumentationPluginTests : IClassFixture<TestHostFixture>
{
    private readonly IPluginService _service;
    private readonly TestHostFixture _fixture;

    public DocumentationPluginTests(TestHostFixture fixture)
    {
        fixture
            .EnablePlugin(builder => builder
                .EnableWebApi()
                .EnableDocumentation<DocumentationPluginBuilderMock>());

        _service = fixture.GetRequiredService<IPluginService>();

        this._fixture = fixture;
    }

    [Fact]
    public void CreatePlugin()
    {
        var plugin = _service.GetPlugin<DocumentationPlugin>();

        Assert.NotNull(plugin);
    }

    [Fact]
    public void GetPluginConfigurations()
    {
        var plugins = _service.GetPlugins<IDocumentationPluginBuilder>();

        Assert.NotNull(plugins);
        Assert.Single(plugins);
    }
}
