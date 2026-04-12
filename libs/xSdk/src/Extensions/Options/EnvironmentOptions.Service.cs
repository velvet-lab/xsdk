using System.Reflection;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Options;

public sealed partial class EnvironmentOptions
{
    [Variable(name: Definitions.ServiceName.Name, template: Definitions.ServiceName.Template, helpText: Definitions.ServiceName.HelpText)]
    public string? ServiceName
    {
        get => ReadValue<string>(Definitions.ServiceName.Name);
        set => SetValue(Definitions.ServiceName.Name, value);
    }

    [Variable(name: Definitions.ServiceNamespace.Name, template: Definitions.ServiceNamespace.Template, helpText: Definitions.ServiceNamespace.HelpText)]
    public string? ServiceNamespace
    {
        get => ReadValue<string>(Definitions.ServiceNamespace.Name);
        set => SetValue(Definitions.ServiceNamespace.Name, value);
    }

    [Variable(name: Definitions.ServiceVersion.Name, template: Definitions.ServiceVersion.Template, helpText: Definitions.ServiceVersion.HelpText)]
    public string? ServiceVersion
    {
        get => ReadValue<string>(Definitions.ServiceVersion.Name);
        set => SetValue(Definitions.ServiceVersion.Name, value);
    }

    [Variable(name: Definitions.ServiceFullName.Name, helpText: Definitions.ServiceFullName.HelpText, protect: true, hidden: true)]
    public string? ServiceFullName { get; private set; }

    private void InitializeService()
    {
        string? currentServiceName = ServiceName;
        string? currentServiceNamespace = ServiceNamespace;
        string currentServiceVersion = ServiceVersion;

        if (string.IsNullOrEmpty(currentServiceName) || string.IsNullOrEmpty(currentServiceNamespace) || string.IsNullOrEmpty(currentServiceVersion))
        {
            Assembly? assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly.GetName();

            if (string.IsNullOrEmpty(currentServiceName))
            {
                currentServiceName = assemblyName.Name;
            }

            if (string.IsNullOrEmpty(currentServiceNamespace))
            {
                currentServiceNamespace = ReadServiceNamespace(assembly);
                if (string.IsNullOrEmpty(currentServiceNamespace))
                {
                    currentServiceNamespace = Definitions.ServiceNamespace.DefaultValue;
                }
            }

            if (string.IsNullOrEmpty(currentServiceVersion))
            {
                currentServiceVersion = assemblyName.Version.ToString();
            }
        }

        if (!string.IsNullOrEmpty(currentServiceName) && !string.IsNullOrEmpty(currentServiceNamespace))
        {
            string seperator = ".";
            if (currentServiceNamespace.EndsWith('.'))
            {
                currentServiceNamespace = currentServiceNamespace.Substring(currentServiceNamespace.Length - 1);
            }

            ServiceFullName = $"{currentServiceNamespace}{seperator}{currentServiceName}".Trim();
        }

        ServiceName = currentServiceName;
        ServiceNamespace = currentServiceNamespace;
        ServiceVersion = currentServiceVersion;
    }

    private static string? ReadServiceNamespace(Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();
        if (types != null && types.Any())
        {
            return types.Select(x => x.Namespace).Where(x => x != null).OrderBy(x => x.Length).FirstOrDefault();
        }
        else
        {
            return string.Empty;
        }
    }

    private static partial class Definitions
    {
        public static class ServiceName
        {
            public const string Name = "service-name";
            public const string Template = "--service-name <name>";
            public const string HelpText = "Service name to identify the application in MaaS environments";
        }

        public static class ServiceNamespace
        {
            public const string Name = "service-namespace";
            public const string Template = "--service-namespace <namespace>";
            public const string HelpText = "Service namespace to identify the application in MaaS environments";
            public const string DefaultValue = "xSdk";
        }

        public static class ServiceVersion
        {
            public const string Name = "service-version";
            public const string Template = "--service-version <version>";
            public const string HelpText = "Service version to identify the application in MaaS environments";
        }

        public static class ServiceFullName
        {
            public const string Name = "service-fullname";
            public const string HelpText = "Fullname for the service identify the application in MaaS environments";
        }
    }
}
