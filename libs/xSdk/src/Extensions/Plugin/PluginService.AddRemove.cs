using xSdk.Shared;
using System.Reflection;

namespace xSdk.Extensions.Plugin
{
    internal partial class PluginService
    {
        public Task AddPluginAsync(Type pluginType, CancellationToken token = default)
        {
            typePluginCatalogs.AddOrNew(pluginType, CatalogHelper.CreateTypeCatalog(pluginType));
            isTypePluginCatalogsStale = true;

            return Task.CompletedTask;
        }

        public Task AddPluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default)
        {
            assemblyPluginCatalogs.AddOrNew(sourceAssembly, CatalogHelper.CreateAssemblyCatalog(sourceAssembly));
            isAssemblyPluginCatalogsStale = true;

            return Task.CompletedTask;
        }

        public Task RemovePluginAsync(Type pluginType, CancellationToken token = default)
        {
            if (typePluginCatalogs.ContainsKey(pluginType))
            {
                isTypePluginCatalogsStale = true;
                typePluginCatalogs.Remove(pluginType);
            }
            return Task.CompletedTask;
        }

        public Task RemovePluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default)
        {
            if (assemblyPluginCatalogs.ContainsKey(sourceAssembly))
            {
                isAssemblyPluginCatalogsStale = true;
                assemblyPluginCatalogs.Remove(sourceAssembly);
            }
            return Task.CompletedTask;
        }
    }
}
