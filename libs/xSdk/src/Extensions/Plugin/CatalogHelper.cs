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
        return new TypeFinderOptions
        {
            TypeFinderCriterias = new List<TypeFinderCriteria> {
                new() { AssignableTo = typeof(IPlugin) }
            }
        };
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
