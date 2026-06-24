/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using xSdk.Extensions.Authentication;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.Authentication;

[ExcludeFromCodeCoverage(Justification = "ASP.NET Core authentication pipeline – requires a running web host.")]
internal sealed class AuthenticationPluginHost(IOptions<ApiKeyPluginOptions> apiKeyOptions, IOptions<EnvironmentOptions> environmentOptions, ILogger<AuthenticationPluginHost> logger) : WebPluginHost
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        AuthenticationBuilder authBuilder = services
            // Add Auth
            .AddAuthentication(_ =>
            {
                _.DefaultScheme = AuthenticationDefaults.MulitAuth.Scheme;
                _.DefaultChallengeScheme = AuthenticationDefaults.MulitAuth.Scheme;
            })
            // Add Policy Scheme to decide which Auth should used
            .AddPolicyScheme(AuthenticationDefaults.MulitAuth.Scheme, AuthenticationDefaults.MulitAuth.Scheme, EnableMultiAuth);

        // API Key Auth is always needed for the default Multi Auth Scheme
        authBuilder.AddApiKeyAuth(apiKeyOptions.Value, environmentOptions.Value);
        InvokeBuilders<IAuthenticationPluginBuilder>(x => x.ConfigureAuthentication(authBuilder));

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

            InvokeBuilders<IAuthenticationPluginBuilder>(x => x.ConfigureAuthorization(_));
        });
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private void EnableMultiAuth(PolicySchemeOptions options)
    {
        logger.LogTrace("Try to find the correct authentication for incomming request");
        options.ForwardDefaultSelector = context =>
        {
            string? scheme = null;
            TryRetrieveAuthenticationScheme(context, out scheme);
            if (!string.IsNullOrEmpty(scheme))
            {
                logger.LogTrace("Found the correct authentication for incomming request: {scheme}", scheme);
                return scheme;
            }

            string? authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                if (authorizationHeader.StartsWith(AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Header))
                {
                    logger.LogTrace("API Key Auth is requested");
                    return AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Scheme;
                }
            }

            // Default Api Auth Scheme
            logger.LogTrace("API Key Auth is requested");
            return AuthenticationDefaults.ApiKeyAuth.InHeader.Scheme;
        };
    }

    private void TryRetrieveAuthenticationScheme(HttpContext context, out string? scheme)
    {
        string? result = null;
        InvokeBuilders<IAuthenticationPluginBuilder>(x => x.TryRetrieveAuthenticationScheme(context, out result));

        scheme = result;
    }
}
