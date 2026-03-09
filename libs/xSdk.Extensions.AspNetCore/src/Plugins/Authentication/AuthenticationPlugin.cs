using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Plugins.Authentication;

internal sealed class AuthenticationPlugin : WebHostPluginBase
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var apiKeySetup = SlimHost.Instance.VariableSystem.GetSetup<ApiKeySetup>();
        var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();

        var authBuilder = services
            // Add Auth
            .AddAuthentication(_ =>
            {
                _.DefaultScheme = AuthenticationDefaults.MulitAuth.Scheme;
                _.DefaultChallengeScheme = AuthenticationDefaults.MulitAuth.Scheme;
            })
            // Add Policy Scheme to decide which Auth should used
            .AddPolicyScheme(AuthenticationDefaults.MulitAuth.Scheme, AuthenticationDefaults.MulitAuth.Scheme, EnableMultiAuth);

        // API Key Auth is always needed for the default Multi Auth Scheme
        authBuilder.AddApiKeyAuth();
        SlimHost.Instance.PluginSystem.Invoke<IAuthenticationPluginBuilder>(x => x.ConfigureAuthentication(authBuilder));

        // Add Client defined Policies
        services.AddAuthorization(_ =>
        {
            // For all Pages/Controllers a.s.o with a Authorize Attribute, but without any Policy
            // e.g. [Authorize]. This Policy will not applied if you use [Authorize Policy= "..."]
            _.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            // For all Pages/Controllers a.s.o without a [Authorize] Attribute
            //_.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();

            SlimHost.Instance.PluginSystem.Invoke<IAuthenticationPluginBuilder>(x => x.ConfigureAuthorization(_));
        });
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private void EnableMultiAuth(PolicySchemeOptions options)
    {
        Logger.Trace("Try to find the correct authentication for incomming request");
        options.ForwardDefaultSelector = context =>
        {
            string? scheme = null;
            TryRetrieveAuthenticationScheme(context, out scheme);
            if (!string.IsNullOrEmpty(scheme))
            {
                Logger.Trace($"Found the correct authentication for incomming request: {scheme}");
                return scheme;
            }

            string? authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                if (authorizationHeader.StartsWith(AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Header))
                {
                    Logger.Trace("API Key Auth is requested");
                    return AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Scheme;
                }
            }

            // Default Api Auth Scheme
            Logger.Trace("API Key Auth is requested");
            return AuthenticationDefaults.ApiKeyAuth.InHeader.Scheme;
        };
    }

    private void TryRetrieveAuthenticationScheme(HttpContext context, out string? scheme)
    {
        string? result = null;
        SlimHost.Instance.PluginSystem.Invoke<IAuthenticationPluginBuilder>(x => x.TryRetrieveAuthenticationScheme(context, out result));

        scheme = result;
    }
}
