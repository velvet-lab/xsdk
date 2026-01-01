using Asp.Versioning.ApiExplorer;
using xSdk.Extensions.Plugin;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace xSdk.Plugins.Documentation.Mocks
{
    internal class DocumentationPluginBuilderMock : PluginBuilderBase, IDocumentationPluginBuilder
    {
        public void ConfigureSwagger(SwaggerGenOptions options) { }

        public void ConfigureSwaggerUi(SwaggerUIOptions options) { }

        public OpenApiInfo CreateApiInfo(ApiVersionDescription description)
        {
            return new OpenApiInfo
            {
                Title = "Fake API",
                Version = description.ApiVersion.ToString()
            };
        }
    }
}
