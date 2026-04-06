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
using xSdk.Extensions.Documentation;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Documentation;

public class DocumentationPluginTests : IClassFixture<TestHostFixture>
{
    private readonly IHost _host;    

    public DocumentationPluginTests(TestHostFixture fixture)
    {
        _host = fixture
            .EnablePlugin(builder => builder
                .EnableWebApi()
                .EnableDocumentation<DocumentationPluginBuilderMock>())
            .BuildHost();
    }

    [Fact]
    public void CreatePlugin()
    {
        var plugin = _host.Services
            .GetRequiredService<IPluginService>()
            .GetPlugin<DocumentationPluginHost>();

        Assert.NotNull(plugin);
    }

    [Fact]
    public void GetPluginConfigurations()
    {
        var plugins = _host.Services
            .GetRequiredService<IPluginService>()
            .GetPlugins<IDocumentationPluginBuilder>();

        Assert.NotNull(plugins);
        Assert.Single(plugins);
    }
}
