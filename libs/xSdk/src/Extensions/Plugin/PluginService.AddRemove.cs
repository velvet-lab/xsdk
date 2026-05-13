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
using xSdk.Tools;

namespace xSdk.Extensions.Plugin;

internal partial class PluginService
{
    public Task AddPluginAsync(Type pluginType, CancellationToken token = default)
    {
        _typePluginCatalogs.AddOrNew(pluginType, CatalogHelper.CreateTypeCatalog(pluginType));
        _isTypePluginCatalogsStale = true;

        return Task.CompletedTask;
    }

    public Task AddPluginsFromAsync(Assembly[] sourceAssemblies, CancellationToken token = default)
    {
        foreach (Assembly sourceAssembly in sourceAssemblies)
        {
            _assemblyPluginCatalogs.AddOrNew(sourceAssembly, CatalogHelper.CreateAssemblyCatalog(sourceAssembly));
            _isAssemblyPluginCatalogsStale = true;
        }

        return Task.CompletedTask;
    }

    public Task RemovePluginAsync(Type pluginType, CancellationToken token = default)
    {
        if (_typePluginCatalogs.ContainsKey(pluginType))
        {
            _isTypePluginCatalogsStale = true;
            _typePluginCatalogs.Remove(pluginType);
        }
        return Task.CompletedTask;
    }

    public Task RemovePluginsFromAsync(Assembly[] sourceAssemblies, CancellationToken token = default)
    {
        foreach (Assembly sourceAssembly in sourceAssemblies)
        {
            if (_assemblyPluginCatalogs.ContainsKey(sourceAssembly))
            {
                _isAssemblyPluginCatalogsStale = true;
                _assemblyPluginCatalogs.Remove(sourceAssembly);
            }
        }
        return Task.CompletedTask;
    }
}
