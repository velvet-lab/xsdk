using System.Reflection;
using Weikio.PluginFramework.Abstractions;
using Weikio.PluginFramework.Catalogs;
using Weikio.PluginFramework.Context;
using Weikio.PluginFramework.TypeFinding;
using xSdk.Extensions.IO;

namespace xSdk.Extensions.Plugin;

internal static class CatalogHelper
{
    internal static FolderPluginCatalog? CreateFolderPluginCatalog(IFileSystemService service, FileSystemContext context, string path)
    {
        var root = service.RequestFileSystem(context);
        if (root != null && root.Data.DirectoryExists(path))
        {
            var realPath = root.Data.GetFullPath(path);
            var options = new FolderPluginCatalogOptions
            {
                TypeFinderOptions = CreateTypeFinderOptions(),
                // PluginLoadContextOptions = new PluginLoadContextOptions,
                // PluginNameOptions = CreatePluginNameOptions()
            };

            return new FolderPluginCatalog(realPath, options);
        }
        return null;
    }

    internal static TypePluginCatalog CreateTypeCatalog(Type pluginType)
    {
        var options = new TypePluginCatalogOptions
        {
            TypeFinderOptions = CreateTypeFinderOptions(),
            // PluginLoadContextOptions = new PluginLoadContextOptions,
            // PluginNameOptions = CreatePluginNameOptions()
        };
        return new TypePluginCatalog(pluginType, options);
    }

    internal static AssemblyPluginCatalog CreateAssemblyCatalog(Assembly sourceAssembly)
    {
        var options = new AssemblyPluginCatalogOptions
        {
            TypeFinderOptions = CreateTypeFinderOptions(),
            // PluginLoadContextOptions = new PluginLoadContextOptions,
            // PluginNameOptions = CreatePluginNameOptions()
        };

        return new AssemblyPluginCatalog(sourceAssembly, options);
    }

    private static TypeFinderOptions CreateTypeFinderOptions()
    {
        return new TypeFinderOptions { TypeFinderCriterias = new List<TypeFinderCriteria> { new TypeFinderCriteria { AssignableTo = typeof(IPlugin) } } };
    }

    private static PluginLoadContextOptions CreatePluginLoadContextOptions()
    {
        return new PluginLoadContextOptions { };
    }

    private static PluginNameOptions CreatePluginNameOptions()
    {
        return new PluginNameOptions { };
    }
}
