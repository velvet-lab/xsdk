using xSdk.Extensions.IO;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Weikio.PluginFramework.Catalogs;

namespace xSdk.Extensions.Plugin
{
    internal partial class PluginService
    {
        private CompositePluginCatalog aggregateCatalog;

        private readonly Dictionary<Type, TypePluginCatalog> typePluginCatalogs = new();
        private bool isTypePluginCatalogsStale = false;

        private readonly Dictionary<Assembly, AssemblyPluginCatalog> assemblyPluginCatalogs = new();
        private bool isAssemblyPluginCatalogsStale = false;

        private async Task LoadPluginsAsync()
        {
            var catalog = await InitialzeCatalogsAsync();

            var abstractPlugins = catalog.GetPlugins().Where(x => x != null);
            foreach (var abstractPlugin in abstractPlugins)
            {
                try
                {
                    if (!plugins.Any(x => x.WeikioPlugin.Type == abstractPlugin.Type))
                    {
                        var item = new PluginItem(abstractPlugin);
                        plugins.Add(item);
                    }
                }
                catch (MissingMethodException mme)
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
            if (isTypePluginCatalogsStale || isAssemblyPluginCatalogsStale || aggregateCatalog == null)
            {
                aggregateCatalog = new CompositePluginCatalog();

                var machineCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.Machine, "/pluginBuilders");
                var userCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.User, "/pluginBuilders");
                var localCatalog = CatalogHelper.CreateFolderPluginCatalog(fsService, FileSystemContext.Local, "/pluginBuilders");

                if (machineCatalog != null)
                {
                    aggregateCatalog.AddCatalog(machineCatalog);
                }

                if (userCatalog != null)
                {
                    aggregateCatalog.AddCatalog(userCatalog);
                }

                if (localCatalog != null)
                {
                    aggregateCatalog.AddCatalog(localCatalog);
                }

                foreach (var kvp in typePluginCatalogs)
                {
                    aggregateCatalog.AddCatalog(kvp.Value);
                }

                foreach (var kvp in assemblyPluginCatalogs)
                {
                    aggregateCatalog.AddCatalog(kvp.Value);
                }
            }

            await aggregateCatalog.Initialize();

            isTypePluginCatalogsStale = false;
            isAssemblyPluginCatalogsStale = false;

            return aggregateCatalog;
        }
    }
}
