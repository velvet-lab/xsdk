using xSdk.Plugins.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xSdk.Plugins.Documentation
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions EnableApiKeyAuthentication(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(
                AuthenticationDefaults.ApiKeyAuth.Name,
                new OpenApiSecurityScheme()
                {
                    Description = "API Key Authentication",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = AuthenticationDefaults.ApiKeyAuth.InHeader.Header,
                }
            );

            options.OperationFilter<AuthorizeCheckOperationFilter>();

            return options;
        }
    }
}
