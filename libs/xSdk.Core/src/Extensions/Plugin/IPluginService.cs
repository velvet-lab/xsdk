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

using System.Reflection;

namespace xSdk.Extensions.Plugin;

public interface IPluginService
{
    TPlugin? GetPlugin<TPlugin>()
        where TPlugin : IPlugin
        => GetPluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

    IList<TPlugin> GetPlugins<TPlugin>()
        where TPlugin : IPlugin
        => GetPluginsAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

    IList<IPlugin> GetPlugins() => GetPluginsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    Task<TPlugin?> GetPluginAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin;

    Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin;

    Task<IList<IPlugin>> GetPluginsAsync(CancellationToken token = default) => GetPluginsAsync<IPlugin>(token);

    void AddPlugin(Type pluginType) => AddPluginAsync(pluginType).ConfigureAwait(false).GetAwaiter().GetResult();

    void AddPlugin<TPlugin>()
        where TPlugin : IPlugin
        => AddPluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

    Task AddPluginAsync(Type pluginType, CancellationToken token = default);

    Task AddPluginAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin
        => AddPluginAsync(typeof(TPlugin), token);

    void AddPluginsFrom<TSource>() => AddPluginsFromAsync<TSource>().ConfigureAwait(false).GetAwaiter().GetResult();

    void AddPluginsFrom(Assembly sourceAssembly) => AddPluginsFromAsync(sourceAssembly).ConfigureAwait(false).GetAwaiter().GetResult();

    Task AddPluginsFromAsync<TSource>(CancellationToken token = default) => AddPluginsFromAsync(typeof(TSource).Assembly, token);

    Task AddPluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default);

    void RemovePlugin(Type pluginType) => RemovePluginAsync(pluginType).ConfigureAwait(false).GetAwaiter().GetResult();

    void RemovePlugin<TPlugin>()
        where TPlugin : IPlugin
        => RemovePluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

    Task RemovePluginAsync(Type pluginType, CancellationToken token = default);

    Task RemovePluginAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin
        => RemovePluginAsync(typeof(TPlugin), token);

    void RemovePluginsFrom<TSource>() => RemovePluginsFromAsync<TSource>().ConfigureAwait(false).GetAwaiter().GetResult();

    void RemovePluginsFrom(Assembly sourceAssembly) => RemovePluginsFromAsync(sourceAssembly).ConfigureAwait(false).GetAwaiter().GetResult();

    Task RemovePluginsFromAsync<TSource>(CancellationToken token = default) => RemovePluginsFromAsync(typeof(TSource).Assembly, token);

    Task RemovePluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default);
}
