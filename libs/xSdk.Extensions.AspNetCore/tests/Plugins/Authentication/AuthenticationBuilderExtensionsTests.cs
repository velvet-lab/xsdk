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

using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Authentication;
using xSdk.Hosting;
using xSdk.Plugins.Authentication.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Authentication;

public class AuthenticationBuilderExtensionsTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void AddApiKeyRepository_RegistersIApiKeyHandler()
    {
        var host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableAuthentication<ApiKeyAuthenticationBuilderMock>())
            .BuildHost();

        var handler = host.Services.GetService<IApiKeyHandler>();

        Assert.NotNull(handler);
        Assert.IsType<TestApiKeyHandler>(handler);
    }
}

