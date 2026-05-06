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
        var service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        var plugin = await service.GetPluginAsync<IPlugin>(TestContext.Current.CancellationToken);

        Assert.Null(plugin);
    }

    [Fact]
    public void GetService_IPluginService_IsRegistered()
    {
        var service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        Assert.NotNull(service);
    }

    [Fact]
    public async Task AddPluginAsync_ThenRemove_PluginCycleWorks()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        // Add a plugin type        
        await service.AddPluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken);

        // Remove the plugin type
        await service.RemovePluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task AddPluginsFromAsync_CurrentAssembly_DoesNotThrow()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        var assembly = typeof(PluginServiceTests).Assembly;
        await service.AddPluginsFromAsync([assembly], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task RemovePluginsFromAsync_NonExistentAssembly_DoesNotThrow()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        // Removing something not added should be a no-op
        var assembly = typeof(PluginServiceTests).Assembly;
        await service.RemovePluginsFromAsync([assembly], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task AddPluginAsync_SameTypeAddedTwice_DoesNotThrow()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        await service.AddPluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken);
        await service.AddPluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken); // idempotent, should not throw
    }
}

/// <summary>Simple stub plugin used only in tests.</summary>
internal class TestPlugin : IPlugin
{
}
