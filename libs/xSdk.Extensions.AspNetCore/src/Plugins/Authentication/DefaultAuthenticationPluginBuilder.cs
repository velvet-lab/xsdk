using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Authentication;

internal class DefaultAuthenticationPluginBuilder : PluginBuilder, IAuthenticationPluginBuilder
{
    public void ConfigureAuthentication(AuthenticationBuilder builder)
    {

    }

    public void ConfigureAuthorization(AuthorizationOptions options)
    {
        // Default OnlyRead
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        // Configure here your Policies which will used in your Controller
        // options.AddProxyPolicies();
    }

    public void TryRetrieveAuthenticationScheme(HttpContext context, out string? scheme)
    {        
        scheme = null;

        string? authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme))
        {
            Logger.LogTrace("Bearer Auth is requested");
            scheme = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
