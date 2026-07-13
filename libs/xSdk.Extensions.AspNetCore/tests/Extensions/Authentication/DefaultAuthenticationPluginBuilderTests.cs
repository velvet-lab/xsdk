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
using xSdk.Hosting;
using xSdk.Plugins.Authentication;
using xSdk.Plugins.WebApi;

namespace xSdk.Extensions.Authentication;

public class DefaultAuthenticationPluginBuilderTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void EnableAuthentication_RegistersAuthenticationSchemeProvider()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableAuthentication(builder => { }))
            .BuildHost();

        // IAuthenticationSchemeProvider is a singleton and resolvable from the root provider
        IAuthenticationSchemeProvider? schemeProvider = host.Services.GetService<IAuthenticationSchemeProvider>();

        Assert.NotNull(schemeProvider);
    }

    [Fact]
    public void ConfigureAuthorization_SetsDefaultPolicy()
    {
        var builder = new AuthBuilder();
        var options = new AuthorizationOptions();

        builder.ConfigureAuthorizationAction?.Invoke(options);

        Assert.NotNull(options.DefaultPolicy);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_NoBearerHeader_ReturnsNullScheme()
    {
        var builder = new AuthBuilder();
        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "";

        string? scheme = builder.SelectAuthenticationSchemeAction?.Invoke(context);

        Assert.Null(scheme);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_WithBearerHeader_ReturnsBearerScheme()
    {
        var builder = new AuthBuilder();
        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer some-token";

        string? scheme = builder.SelectAuthenticationSchemeAction?.Invoke(context);

        Assert.Equal("Bearer", scheme);
    }
}
