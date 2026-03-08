using System.Reflection;
using NLog;
using xSdk.Extensions.IO;
using xSdk.Hosting;

namespace xSdk.Shared;

internal static class AssemblyCollector
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private static List<Assembly>? _assemblies;

    internal static List<Assembly> Collect()
    {
        _logger.Info("Collect loaded Assemblies");

        if (_assemblies == null || _assemblies.Count == 0)
        {
            _assemblies = new List<Assembly>();

            _logger.Debug("Add assemblies from executing folder");
            AddAssembliesFromExecutingFolder(_assemblies);

            _logger.Debug("Add assemblies from loaded plugins");
            var plugins = SlimHost.Instance.PluginSystem.GetPlugins();
            foreach (var plugin in plugins)
            {
                var assembly = plugin.GetType().Assembly;
                AddAssembly(_assemblies, assembly);
            }
        }

        return _assemblies;
    }

    private static void AddAssembly(List<Assembly> assemblies, Assembly assembly)
    {
        var assemblyNames = assemblies.Select(x => x.FullName).Where(x => x != null);
        var name = assembly.FullName;
        if (!string.IsNullOrEmpty(name) && !assemblyNames.Contains(name))
        {
            assemblies.Add(assembly);

            _logger.Debug("Add referenced Assemblies for found Assemblies");
            AddReferencedAssemblies(assemblies, assembly);
        }
    }

    private static void AddReferencedAssemblies(List<Assembly> assemblies, Assembly assembly)
    {
        var referencedAssemblyNames = assembly.GetReferencedAssemblies();
        var filteredReferencedAssemblyNames = referencedAssemblyNames
            .Where(x => !IsBlacklisted(x.Name))
            .Where(x => !assemblies.Any(y => y.FullName == x.FullName));

        if (filteredReferencedAssemblyNames.Any())
        {
            try
            {
                filteredReferencedAssemblyNames
                    .ToList()
                    .ForEach(x => AddAssembly(assemblies, Assembly.Load(x)));
            }
            catch
            {
                // nothing to tell
            }
        }
    }

    private static void AddAssembliesFromExecutingFolder(List<Assembly> assemblies)
    {
        var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var executingFolder = FileSystemHelper.GetExecutingFolder();
        var assemblyFiles = new DirectoryInfo(executingFolder)
            .EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly)
            .Where(x => !IsBlacklisted(x.Name));

        assemblyFiles
            .Select(x => appDomainAssemblies.FirstOrDefault(y => x.FullName == y.Location)!)
            .Where(x => x != null)
            .Distinct()
            .ToList()
            .ForEach(x => AddAssembly(assemblies, x));
    }

    private static bool IsBlacklisted(string? name)
    {
        var pattern = new string[]
        {
            "Asp",
            "AspNetCore",
            "AutoMapper",
            "Bogus",
            "CloudNative",
            "CommunityToolkit",
            "Consul",
            "FluentValidation",
            "Google",
            "Grpc",
            "Handlebars",
            "Hellang",
            "LiteDB",
            "MicroElements",
            "Microsoft",
            "MongoDB",
            "NLog",
            "NWebsec",
            "Newtonsoft",
            "OpenTelemetry",
            "RestSharp",
            "SemanticVersioning",
            "Sewer56",
            "Spectre",
            "Swashbuckle",
            "System",
            "VaultSharp",
            "Weikio",
            "YamlDotNet",
            "Zio",
            "netcore",
            "netstandard",
        };

        if (!string.IsNullOrEmpty(name))
        {
            return pattern.Any(x => name.StartsWith(x));
        }

        return false;
    }
}
