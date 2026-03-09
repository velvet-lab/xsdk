using System.Reflection;
using xSdk.Shared;

namespace xSdk.Extensions.Plugin;

internal partial class PluginService
{
    public Task AddPluginAsync(Type pluginType, CancellationToken token = default)
    {
        _typePluginCatalogs.AddOrNew(pluginType, CatalogHelper.CreateTypeCatalog(pluginType));
        _isTypePluginCatalogsStale = true;

        return Task.CompletedTask;
    }

    public Task AddPluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default)
    {
        _assemblyPluginCatalogs.AddOrNew(sourceAssembly, CatalogHelper.CreateAssemblyCatalog(sourceAssembly));
        _isAssemblyPluginCatalogsStale = true;

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

    public Task RemovePluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default)
    {
        if (_assemblyPluginCatalogs.ContainsKey(sourceAssembly))
        {
            _isAssemblyPluginCatalogsStale = true;
            _assemblyPluginCatalogs.Remove(sourceAssembly);
        }
        return Task.CompletedTask;
    }
}
