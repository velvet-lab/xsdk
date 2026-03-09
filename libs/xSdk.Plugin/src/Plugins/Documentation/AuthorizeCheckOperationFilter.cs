using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using xSdk.Plugins.Authentication;

namespace xSdk.Plugins.Documentation;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize =
            context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        if (hasAuthorize)
        {
            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", CreateProblemResponse("Unauthorized"));
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", CreateProblemResponse("Forbidden"));
            }

            var requirements = new List<OpenApiSecurityRequirement>();
            requirements.Add(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference { Id = AuthenticationDefaults.ApiKeyAuth.Name, Type = ReferenceType.SecurityScheme },
                        },
                        new List<string>()
                    },
                }
            );

            operation.Security = requirements;
        }
    }

    private OpenApiResponse CreateProblemResponse(string message)
    {
        return new OpenApiResponse
        {
            Description = message,
            Content = new Dictionary<string, OpenApiMediaType>()
            {
                {
                    "application/json", new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "ProblemDetails",
                                Type = ReferenceType.Schema
                            }
                        }
                    }
                }
            }
        };
    }
}
