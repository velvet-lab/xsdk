using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;
using xSdk.Extensions.Plugin;
using xSdk.Plugins.Documentation;

namespace xSdk.Demos.Builders;

public class DocumentationPluginBuilder : PluginBuilderBase, IDocumentationPluginBuilder
{
    public OpenApiInfo CreateApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Sample API",
            Version = description.ApiVersion.ToString(),
            Description = "Sample API Documentation.",
            License = new OpenApiLicense { Name = "MIT" },
        };

        if (description.GroupName == "v2")
        {
            info.Title = "Sample API Test";
        }

        if (description.GroupName == "v3")
        {
            info.Title = "Sample API with HATEOAS Links";
        }

        if (description.IsDeprecated)
        {
            info.Description += " [This API version has been deprecated]";
        }

        return info;
    }
}
