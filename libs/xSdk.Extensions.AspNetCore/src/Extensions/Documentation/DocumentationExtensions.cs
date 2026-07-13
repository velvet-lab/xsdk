using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;

namespace xSdk.Extensions.Documentation;

public static class DocumentationExtensions
{
    extension(DocumentationBuilder builder)
    {
        public DocumentationBuilder WithApiInfo(Func<ApiVersionDescription, OpenApiInfo> createApiInfoAction)
        {
            builder.CreateApiInfoAction = createApiInfoAction;
            return builder;
        }
    }
}
