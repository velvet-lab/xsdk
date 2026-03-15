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
