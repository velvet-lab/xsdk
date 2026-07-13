using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using xSdk.Extensions.Builder;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.Authentication;

public class AuthBuilder : BuilderBase
{
    private static ILogger Logger => field ??= LogManager.CreateLogger<AuthBuilder>();

    internal Action<AuthenticationBuilder>? ConfigureAuthenticationAction;
    internal Action<AuthorizationOptions>? ConfigureAuthorizationAction { get => field ??= ConfigureAuthorization; set; }
    internal Func<HttpContext, string?>? SelectAuthenticationSchemeAction { get => field ??= SelectAuthenticationScheme; set; }

    private static void ConfigureAuthorization(AuthorizationOptions options)
    {
        // Default OnlyRead
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        // ConfigureBuilder here your Policies which will used in your Controller
        // options.AddProxyPolicies();
    }

    private static string? SelectAuthenticationScheme(HttpContext context)
    {
        string? scheme = null;

        string? authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme))
        {
            Logger.LogTrace("Bearer Auth is requested");
            scheme = JwtBearerDefaults.AuthenticationScheme;
        }

        return scheme;
    }
}
