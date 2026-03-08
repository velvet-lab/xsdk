using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Authentication;

[CLSCompliant(false)]
public interface IAuthenticationPluginBuilder : IPluginBuilder
{
    void ConfigureAuthentication(AuthenticationBuilder builder);

    void ConfigureAuthorization(AuthorizationOptions options);

    void TryRetrieveAuthenticationScheme(HttpContext context, out string? scheme);
}
