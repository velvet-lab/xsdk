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

using Microsoft.Extensions.Logging;
using xSdk.Extensions.IO;

namespace xSdk.Extensions.Plugin;

internal partial class PluginService(IFileSystemService fsService, IServiceProvider provider, ILogger<PluginService> logger) : IPluginService
{
    private readonly List<PluginItem> _plugins = new();
    
    public Task<TPlugin?> GetPluginAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin
        => GetPluginsAsync<TPlugin>(token)
        .ContinueWith(task => task.Result.FirstOrDefault());

    public async Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(CancellationToken token = default)
        where TPlugin : IPlugin
    {
        var searchResult = new List<PluginItem>();

        await LoadPluginsAsync();
        foreach (var item in _plugins)
        {
            if (item.Plugin is TPlugin concretePlugin)
            {
                searchResult.Add(item);
            }
        }

        return searchResult.OrderBy(x => x.Order).Select(x => x.Plugin).Cast<TPlugin>().ToList();
    }
}
