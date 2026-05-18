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
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public class PluginServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void LoadPlugins()
    {
        IPluginService? service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        IList<IPlugin>? plugins = service?.GetPlugins();

        Assert.NotNull(plugins);
        Assert.False(plugins.Any());
    }

    [Fact]
    public async Task GetPluginAsync_NoPluginsLoaded_ReturnsNull()
    {
        IPluginService? service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        if (service != null)
        {
            IPlugin? plugin = await service.GetPluginAsync<IPlugin>(TestContext.Current.CancellationToken);
            Assert.Null(plugin);
        }
    }

    [Fact]
    public void GetService_IPluginService_IsRegistered()
    {
        IPluginService? service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        Assert.NotNull(service);
    }
}

/// <summary>Simple stub plugin used only in tests.</summary>
internal class TestPlugin : IPlugin
{
}
