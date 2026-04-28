using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Options;

public sealed class ApplicationOptions
{
    public ApplicationOptions()
    {
        var version = new SemVer(GetType().Assembly.GetName().Version.ToString());
        Version = version;
        AppVersion = version.ToString();
    }

    [Variable(
        name: Definitions.AppName.Name,
        helpText: Definitions.AppName.HelpText,
        defaultValue: Definitions.AppName.DefaultValue,
        resourceNames: ["app.name"],
        hidden: true
    )]
    public string Name { get; set; }

    [Variable(
        name: Definitions.AppDescription.Name,
        helpText: Definitions.AppDescription.HelpText,
        resourceNames: ["app.description"],
        hidden: true
    )]
    public string Description { get; set; }

    [Variable(
        name: Definitions.AppCompany.Name,
        helpText: Definitions.AppCompany.HelpText,
        defaultValue: Definitions.AppCompany.DefaultValue,
        resourceNames: ["app.company"],
        hidden: true
    )]
    public string Company { get; set; }

    [Variable(name: Definitions.AppVersion.Name, helpText: Definitions.AppVersion.HelpText, resourceNames: ["app.version"], hidden: true)]
    public string AppVersion { get; set; }

    [Variable(
        name: Definitions.AppPrefix.Name,
        helpText: Definitions.AppPrefix.HelpText,
        defaultValue: Definitions.AppPrefix.DefaultValue,
        resourceNames: ["app.prefix"],
        hidden: true
    )]
    public string Prefix { get; set; }

    public SemVer Version { get; private set; }

    internal static partial class Definitions
    {
        public static class AppName
        {
            public const string Name = "app-name";
            public const string HelpText = "Short name of the application";
            public const string DefaultValue = "xsdk";
        }

        public static class AppDescription
        {
            public const string Name = "app-description";
            public const string HelpText = "Description of the application";
        }

        public static class AppCompany
        {
            public const string Name = "app-company";
            public const string HelpText = "Company name of the application";
            public const string DefaultValue = "xcom";
        }

        public static class AppPrefix
        {
            public const string Name = "app-prefix";
            public const string HelpText = "Prefix for the application";
            public const string DefaultValue = "XSDK";
        }

        public static class AppVersion
        {
            public const string Name = "app-version";
            public const string HelpText = "Version of the application";
        }
    }
}
