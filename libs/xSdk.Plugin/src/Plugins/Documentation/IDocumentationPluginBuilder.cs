using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Documentation;

public interface IDocumentationPluginBuilder : IPluginBuilder
{
    OpenApiInfo CreateApiInfo(ApiVersionDescription description);
}
