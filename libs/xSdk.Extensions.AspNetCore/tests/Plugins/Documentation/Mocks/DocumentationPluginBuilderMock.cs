using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Documentation.Mocks;

internal class DocumentationPluginBuilderMock : PluginBuilderBase, IDocumentationPluginBuilder
{
    public OpenApiInfo CreateApiInfo(ApiVersionDescription description)
    {
        return new OpenApiInfo
        {
            Title = "Fake API",
            Version = description.ApiVersion.ToString()
        };
    }
}
