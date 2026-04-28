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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using xSdk.Extensions.Authentication;
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
