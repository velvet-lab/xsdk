using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Documentation;

public class DocumentationBuilder : BuilderBase
{
    internal DocumentationOptions Options => SlimServices.GetRequiredService<IOptions<DocumentationOptions>>().Value;

    internal Func<ApiVersionDescription, OpenApiInfo> CreateApiInfoAction
    {
        get => field ?? (_ => CreateApiInfo(_));
        set;
    }

    private OpenApiInfo CreateApiInfo(ApiVersionDescription description)
        => new()
        {
            Title = "SDK API Documentation",
            Version = "v1",
            Description =
                "Default API Documentation for xSDK. Convert replace the default Documentation use the IDocumentationPluginBuilder Interface while the plugin will enabled.",
            License = new OpenApiLicense { Name = "MIT" },
        };
}
