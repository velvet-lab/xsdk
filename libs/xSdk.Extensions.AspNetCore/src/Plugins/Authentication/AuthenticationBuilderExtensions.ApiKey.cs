using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Authentication;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Plugins.Authentication;

public static partial class AuthenticationBuilderExtensions
{
    internal static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder)
    {
        var apiKeySetup = SlimHost.Instance.VariableSystem.GetSetup<ApiKeySetup>();
        var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();

        // Add ApiKeyName Auth
        builder
            .AddApiKeyInHeader(AuthenticationDefaults.ApiKeyAuth.InHeader.Scheme, options => ActivateInHeader(options, envSetup, apiKeySetup))
            .AddApiKeyInAuthorizationHeader(
                AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Scheme,
                options => ActivateInAuthorizationHeader(options, envSetup, apiKeySetup)
            );

        return builder;
    }

    private static void ActivateInHeader(ApiKeyOptions options, EnvironmentSetup envSetup, ApiKeySetup apiKeySetup)
    {
        options.KeyName = AuthenticationDefaults.ApiKeyAuth.InHeader.Header;

        EnableApiKeyAuth(options, envSetup, apiKeySetup);
    }

    private static void ActivateInAuthorizationHeader(ApiKeyOptions options, EnvironmentSetup envSetup, ApiKeySetup apiKeySetup)
    {
        options.KeyName = AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Header;

        EnableApiKeyAuth(options, envSetup, apiKeySetup);
    }

    private static void EnableApiKeyAuth(ApiKeyOptions options, EnvironmentSetup envSetup, ApiKeySetup apiKeySetup)
    {
        options.Realm = apiKeySetup.Realm;

        //// Optional option to suppress the browser login dialog for ajax calls.
        options.SuppressWWWAuthenticateHeader = true;

        //// Optional option to ignore extra check of ApiKeyName string after it is validated.
        //options.ForLegacyIgnoreExtraValidatedApiKeyCheck = true;

        // Optional option to ignore authentication if AllowAnonumous metadata/filter attribute is added to an endpoint.
        options.IgnoreAuthenticationIfAllowAnonymous = true;

        // Optional events to override the ApiKeyName original logic with custom logic.
        // Only use this if you know what you are doing at your own risk. Any of the events can be assigned.
        options.Events = new ApiKeyEvents
        {
            OnValidateKey = context => ValidateKeyAsync(context, envSetup),
            OnHandleChallenge = HandleChallengeAsync,
            OnHandleForbidden = HandleForbidden,
            OnAuthenticationFailed = AuthenticationFailedAsync,
            OnAuthenticationSucceeded = context =>
            {
                // Will be invoked after a successful authentication.
                context.Response.Headers.Append("AuthenticationCustomHeader", "Convert OnAuthenticationSucceeded");
                return Task.CompletedTask;
            },
        };
    }

    private static async Task ValidateKeyAsync(ApiKeyValidateKeyContext context, EnvironmentSetup envSetup)
    {
        // Will be invoked just before validating the api key.
        IApiKey? apiKey = null;

        var repos = context.HttpContext.RequestServices.GetServices<IApiKeyHandler>();
        if (repos != null && repos.Any())
        {
            foreach (var repo in repos)
            {
                apiKey = await repo.GetApiKeyAsync(context.ApiKey);
                if (apiKey != null && !string.IsNullOrEmpty(apiKey.Key))
                {
                    break;
                }
            }
        }

        if (apiKey != null)
        {
            if (apiKey.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(apiKey.OwnerName))
                {
                    if (apiKey.Claims != null && apiKey.Claims.Any())
                    {
                        context.ValidationSucceeded(apiKey.OwnerName, apiKey.Claims);
                    }
                    else
                    {
                        context.ValidationFailed("No claims associated with this API Key");
                    }
                }
                else
                {
                    context.ValidationFailed("No Owner associated with this API Key");
                }
            }
            else
            {
                context.ValidationFailed("API Key does not match/is not valid");
            }
        }
        else
        {
            Logger.Warn("API Key could not validated, because no Authorization Service is available");
        }
    }

    private static async Task HandleChallengeAsync(ApiKeyHandleChallengeContext context)
    {
        // Will be invoked before a challenge is sent back to the caller when handling unauthorized response
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        var details = new ProblemDetails
        {
            Title = "Access could not verified",
            Detail = "API Key does not match or is missing. Please define a valid API Key to access the API.",
            Status = StatusCodes.Status401Unauthorized,
        };
        await context.Response.WriteAsJsonAsync(details);

        context.Handled(); // important! do not forget to call this method at the end.
    }

    private static async Task HandleForbidden(ApiKeyHandleForbiddenContext context)
    {
        // Will be invoked if Authorization fails and results in a Forbidden response.
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        var result = new ProblemDetails
        {
            Title = "Access is forbidden",
            Detail = "Given API Key has no rights to access the Api",
            Status = StatusCodes.Status403Forbidden,
        };
        await context.Response.WriteAsJsonAsync(result);

        context.Handled(); // important! do not forget to call this method at the end.
    }

    private static Task AuthenticationFailedAsync(ApiKeyAuthenticationFailedContext context)
    {
        // Will be invoked when the authentication fails
        // After this call 'HandleChallengeAsync' will called,

        context.Fail("Failed to authenticate. It seems your API Key is wrong.");

        return Task.CompletedTask;
    }
}
