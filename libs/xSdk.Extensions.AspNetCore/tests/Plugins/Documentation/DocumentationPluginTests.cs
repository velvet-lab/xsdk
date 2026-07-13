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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Documentation;

public class DocumentationPluginTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    private readonly IHost _host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableDocumentation())
            .BuildHost();

    [Fact]
    public void CreatePlugin()
    {
        PluginHost? pluginHost = _host.Services
            .GetRequiredService<IPluginService>()
            .GetPlugin<xSdk.Plugins.Documentation.PluginHost>();

        Assert.NotNull(pluginHost);
    }
}
