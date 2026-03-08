using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using xSdk.Extensions.Plugin;
using xSdk.Plugins.Authentication;

namespace xSdk.Demos.Builders;

internal class AuthenticationPluginBuilder : PluginBuilderBase, IAuthenticationPluginBuilder
{
    // Global Constants for an easier handling
    public const string Policy_OnlyRead = "OnlyRead";
    public const string Policy_ReadAndWrite = "ReadAndWrite";

    public void ConfigureAuthentication(AuthenticationBuilder builder)
    {
        builder
            .AddApiKeyRepository<ApiKeyHandler>()
            .AddApiKeyRepository<ApiKeyHandlerTwo>();

    }

    public void ConfigureAuthorization(AuthorizationOptions options)
    {
        // Configure here your Policies which will used in your Controller
        options.AddPolicy(
            Policy_ReadAndWrite,
            policy =>
            {
                policy.RequireClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Write);
            }
        );

        options.AddPolicy(
            Policy_OnlyRead,
            policy =>
            {
                policy.RequireClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read);
            }
        );
    }

    public void TryRetrieveAuthenticationScheme(HttpContext context, out string? scheme)
    {
        scheme = null;
    }
}
