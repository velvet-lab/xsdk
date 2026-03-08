using System.Reflection;
using NLog;

namespace xSdk.Shared;

public class EmbeddedResourceLoader(Assembly assembly, string @namespace)
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private readonly Assembly _assembly = assembly;
    private readonly string _namespace = @namespace;

    public bool TryReadResource(string fileName, out string content)
    {
        var resourceName = FormatResourceName(_namespace, fileName);

        content = null;
        using (var stream = _assembly.GetManifestResourceStream(resourceName))
        {
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                content = reader.ReadToEnd();
                return true;
            }
            else
            {
                _logger.Trace("Resource '{0}' not found in Assembly", resourceName);
            }
        }

        return false;
    }

    public bool TryReadBinaryResource(string fileName, out byte[] buffer)
    {
        var resourceName = FormatResourceName(_namespace, fileName);

        buffer = new byte[0];
        using (var stream = _assembly.GetManifestResourceStream(resourceName))
        {
            if (stream != null)
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return true;
            }
            else
            {
                _logger.Trace("Resource '{0}' not found in Assembly", resourceName);
            }
        }

        return false;
    }

    private static string FormatResourceName(string resourceNamespace, string resourceName)
    {
        _logger.Trace($"Create resource name for '{0}'", resourceName);

        string fileName = "";
        if (resourceName.IndexOf("/") > -1)
        {
            fileName = resourceName.Substring(resourceName.LastIndexOf("/") + 1);
            fileName = $".{fileName}";
            resourceName = resourceName.Substring(0, resourceName.LastIndexOf("/"));
        }

        resourceName = resourceName.Replace("/", "$");
        var parts = resourceName.Split("$").ToList();
        var items = new List<string>();
        parts.ForEach(x =>
        {
            if (Version.TryParse(x, out Version version))
            {
                var versionAsString = $"_{version.Major}._{version.Minor}._{version.Build}";
                if (version.Revision > 0)
                    versionAsString = $"{versionAsString}._{version.Revision}";

                items.Add(versionAsString);
            }
            else
            {
                items.Add(x.Replace(" ", "_").Replace("\\", ".").Replace("/", ".").Replace("-", "_"));
            }
        });

        var result = $"{resourceNamespace}.{items.Aggregate((a, b) => a + "." + b)}{fileName}";
        _logger.Trace($"Resourcename for '{0}' is '{1}'", resourceName, result);

        return result;
    }
}
