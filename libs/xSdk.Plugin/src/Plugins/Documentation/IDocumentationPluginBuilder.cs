using Asp.Versioning.ApiExplorer;
using xSdk.Extensions.Plugin;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace xSdk.Plugins.Documentation
{
    public interface IDocumentationPluginBuilder : IPluginBuilder
    {
        void ConfigureSwagger(SwaggerGenOptions options);

        void ConfigureSwaggerUi(SwaggerUIOptions options);

        OpenApiInfo CreateApiInfo(ApiVersionDescription description);
    }
}
