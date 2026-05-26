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
    public async Task LoadPlugins()
    {
        IPluginService? service = fixture
            .BuildHost()
            .Services.GetService<IPluginService>();

        if (service != null)
        {
            IList<IPlugin>? plugins = await service.GetPluginsAsync(TestContext.Current.CancellationToken);

            Assert.NotNull(plugins);
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

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Generische Überladung bevorzugen, wenn der Typ bekannt ist", Justification = "<Ausstehend>")]
    public async Task AddPluginAsync_SingleType_IsIncludedInGetPlugins()
    {
        IPluginService service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        await service.AddPluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken);

        IList<IPlugin> plugins = await service.GetPluginsAsync(TestContext.Current.CancellationToken);
        Assert.NotEmpty(plugins);
    }

    [Fact]
    public async Task AddPluginsFromAsync_CurrentAssembly_LoadsPlugins()
    {
        IPluginService service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        await service.AddPluginsFromAsync([typeof(TestPlugin).Assembly], TestContext.Current.CancellationToken);

        IList<IPlugin> plugins = await service.GetPluginsAsync(TestContext.Current.CancellationToken);
        Assert.NotEmpty(plugins);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Generische Überladung bevorzugen, wenn der Typ bekannt ist", Justification = "<Ausstehend>")]
    public async Task RemovePluginAsync_NonExistentType_DoesNotThrow()
    {
        IPluginService service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        try
        {
            await service.RemovePluginAsync(typeof(TestPlugin), TestContext.Current.CancellationToken);

            // No exception expected
        }
        catch (Exception ex)
        {
            Assert.Fail($"Expected no exception, but got: {ex}");
        }
    }

    [Fact]
    public async Task RemovePluginsFromAsync_NonExistentAssembly_DoesNotThrow()
    {
        IPluginService service = fixture
            .BuildHost()
            .Services.GetRequiredService<IPluginService>();

        try
        {
            await service.RemovePluginsFromAsync([typeof(TestPlugin).Assembly], TestContext.Current.CancellationToken);

            // No exception expected
        }
        catch (Exception ex)
        {
            Assert.Fail($"Expected no exception, but got: {ex}");
        }
    }
}

/// <summary>Simple stub plugin used only in tests.</summary>
public class TestPlugin : IPlugin
{
}
