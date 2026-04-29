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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Authentication;
using xSdk.Hosting;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Authentication;

public class DefaultAuthenticationPluginBuilderTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void EnableAuthentication_RegistersAuthenticationService()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableAuthentication())
            .BuildHost();

        IAuthenticationService? authService = host.Services.GetService<IAuthenticationService>();

        Assert.NotNull(authService);
    }

    [Fact]
    public void ConfigureAuthorization_SetsDefaultPolicy()
    {
        DefaultAuthenticationPluginBuilder builder = new DefaultAuthenticationPluginBuilder();
        AuthorizationOptions options = new AuthorizationOptions();

        builder.ConfigureAuthorization(options);

        Assert.NotNull(options.DefaultPolicy);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_NoBearerHeader_ReturnsNullScheme()
    {
        DefaultAuthenticationPluginBuilder builder = new DefaultAuthenticationPluginBuilder();
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "";

        builder.TryRetrieveAuthenticationScheme(context, out string? scheme);

        Assert.Null(scheme);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_WithBearerHeader_ReturnsBearerScheme()
    {
        DefaultAuthenticationPluginBuilder builder = new DefaultAuthenticationPluginBuilder();
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer some-token";

        builder.TryRetrieveAuthenticationScheme(context, out string? scheme);

        Assert.Equal("Bearer", scheme);
    }
}
