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
using xSdk.Extensions.Authentication;

namespace xSdk.Plugins.Authentication;

public class DefaultAuthenticationPluginBuilderTests
{
    private static DefaultAuthenticationPluginBuilder CreateBuilder() => new DefaultAuthenticationPluginBuilder();

    [Fact]
    public void ConfigureAuthentication_WithBuilder_DoesNotThrow()
    {
        var builder = CreateBuilder();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddOptions();
        services.AddAuthentication();
        var authBuilder = new AuthenticationBuilder(services);

        // Should not throw
        builder.ConfigureAuthentication(authBuilder);
    }

    [Fact]
    public void ConfigureAuthorization_SetsDefaultPolicy()
    {
        var builder = CreateBuilder();
        var options = new AuthorizationOptions();

        builder.ConfigureAuthorization(options);

        Assert.NotNull(options.DefaultPolicy);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_NoBearerHeader_ReturnsNullScheme()
    {
        var builder = CreateBuilder();
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "";

        builder.TryRetrieveAuthenticationScheme(context, out var scheme);

        Assert.Null(scheme);
    }

    [Fact]
    public void TryRetrieveAuthenticationScheme_WithBearerHeader_ReturnsBearerScheme()
    {
        var builder = CreateBuilder();
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer some-token";

        builder.TryRetrieveAuthenticationScheme(context, out var scheme);

        Assert.Equal("Bearer", scheme);
    }
}
