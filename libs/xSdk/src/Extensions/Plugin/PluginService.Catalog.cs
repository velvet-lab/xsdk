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
using Microsoft.Extensions.Logging;
using Weikio.PluginFramework.Catalogs;
using xSdk.Extensions.IO;

namespace xSdk.Extensions.Plugin;

internal partial class PluginService
{
    private CompositePluginCatalog _aggregateCatalog;

    private readonly Dictionary<Type, TypePluginCatalog> _typePluginCatalogs = new();
    private bool _isTypePluginCatalogsStale = false;

    private readonly Dictionary<Assembly, AssemblyPluginCatalog> _assemblyPluginCatalogs = new();
    private bool _isAssemblyPluginCatalogsStale = false;

    private async Task LoadPluginsAsync()
    {
        var catalog = await InitialzeCatalogsAsync();

        var abstractPlugins = catalog.GetPlugins().Where(x => x != null);
        foreach (var abstractPlugin in abstractPlugins)
        {
            try
            {
                if (!_plugins.Any(x => x.WeikioPlugin.Type == abstractPlugin.Type))
                {
                    var item = new PluginItem(abstractPlugin, provider);
                    _plugins.Add(item);
                }
            }
            catch (MissingMethodException)
            {
                // Ignore this type of Exception, because this Exception is thrown
                // if a class does not have a default constructor.
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create plugin instance");
            }
        }
    }

    private async Task<CompositePluginCatalog> InitialzeCatalogsAsync()
    {
        if (_isTypePluginCatalogsStale || _isAssemblyPluginCatalogsStale || _aggregateCatalog == null)
        {
            _aggregateCatalog = new CompositePluginCatalog();

            var machineCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.Machine, "/plugins");
            var userCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.User, "/plugins");
            var localCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.Local, "/plugins");

            if (machineCatalog != null)
            {
                _aggregateCatalog.AddCatalog(machineCatalog);
            }

            if (userCatalog != null)
            {
                _aggregateCatalog.AddCatalog(userCatalog);
            }

            if (localCatalog != null)
            {
                _aggregateCatalog.AddCatalog(localCatalog);
            }

            foreach (var kvp in _typePluginCatalogs)
            {
                _aggregateCatalog.AddCatalog(kvp.Value);
            }

            foreach (var kvp in _assemblyPluginCatalogs)
            {
                _aggregateCatalog.AddCatalog(kvp.Value);
            }
        }

        await _aggregateCatalog.Initialize();

        _isTypePluginCatalogsStale = false;
        _isAssemblyPluginCatalogsStale = false;

        return _aggregateCatalog;
    }
}
