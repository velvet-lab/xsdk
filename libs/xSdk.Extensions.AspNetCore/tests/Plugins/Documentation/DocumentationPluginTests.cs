using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xSdk.Plugins.Documentation
{
    public class DocumentationPluginTests : IClassFixture<TestHostFixture>
    {
        private readonly IPluginService service;
        private readonly TestHostFixture fixture;

        public DocumentationPluginTests(TestHostFixture fixture)
        {
            fixture
                .EnablePlugin(builder => builder
                    .EnableWebApi()
                    .EnableDocumentation<DocumentationPluginBuilderMock>());

            service = fixture.GetRequiredService<IPluginService>();

            this.fixture = fixture;
        }

        [Fact]
        public void CreatePlugin()
        {
            var plugin = service.GetPlugin<DocumentationPlugin>();

            Assert.NotNull(plugin);
        }

        [Fact]
        public void GetPluginConfigurations()
        {
            var plugins = service.GetPlugins<IDocumentationPluginBuilder>();

            Assert.NotNull(plugins);
            Assert.Single(plugins);
        }

        [Fact]
        public void LoadSwaggerSchemaGenerator()
        {
            var schemaGenerator = this.fixture.GetRequiredService<ISchemaGenerator>();

            Assert.NotNull(schemaGenerator);
        }
    }
}
