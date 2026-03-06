using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Package
{
    public sealed class PackageSetup : Setup
    {
        [Variable(name: Definitions.Cache.Name, template: Definitions.Cache.Template, helpText: Definitions.Cache.HelpText, prefix: Definitions.Cache.Prefix)]
        public string Cache
        {
            get => this.ReadValue<string>(Definitions.Cache.Name);
            set => this.SetValue(Definitions.Cache.Name, value);
        }

        [Variable(
            name: Definitions.DisableCache.Name,
            template: Definitions.DisableCache.Template,
            helpText: Definitions.DisableCache.HelpText,
            prefix: Definitions.DisableCache.Prefix
        )]
        public bool DisableCache
        {
            get => this.ReadValue<bool>(Definitions.DisableCache.Name);
            set => this.SetValue(Definitions.DisableCache.Name, value);
        }

        [Variable(
            name: Definitions.DisableArtifactory.Name,
            template: Definitions.DisableArtifactory.Template,
            helpText: Definitions.DisableArtifactory.HelpText,
            prefix: Definitions.DisableArtifactory.Prefix
        )]
        public bool DisableArtifactory
        {
            get => this.ReadValue<bool>(Definitions.DisableArtifactory.Name);
            set => this.SetValue(Definitions.DisableArtifactory.Name, value);
        }

        [Variable(
            name: Definitions.Url.Name,
            template: Definitions.Url.Template,
            helpText: Definitions.Url.HelpText,
            prefix: Definitions.Url.Prefix,
            defaultValue: Definitions.Url.DefaultValue
        )]
        public string Url
        {
            get => this.ReadValue<string>(Definitions.Url.Name);
            set => this.SetValue(Definitions.Url.Name, value);
        }

        protected override void Initialize()
        {
            if (string.IsNullOrEmpty(Cache))
            {
                //var runtime = ContainerServiceFactory.CreateService().GetRuntime();
                //Path = runtime.Folders.WorkspaceCache.Location.FullName;
            }
        }

        protected override void ValidateSetup()
        {
            if (!DisableArtifactory)
            {
                this.ValidateMember(x => string.IsNullOrEmpty(x.Url), "Artifactory url is missing");
            }
        }

        internal Uri BaseUrl => new Uri(Url + "artifactory/");

        public static class Definitions
        {
            public static class Cache
            {
                public const string Name = "cache";
                public const string Template = "--cache <cache>";
                public const string HelpText = "Location for the package cache";
                public const string Prefix = "package";
            }

            public static class DisableCache
            {
                public const string Name = "disable-cache";
                public const string Template = "--disable-cache";
                public const string HelpText = "Disables the package cache";
                public const string Prefix = "package";
            }

            public static class DisableArtifactory
            {
                public const string Name = "disable-artifactory";
                public const string Template = "--disable-artifactory";
                public const string HelpText = "Disables the global artifactory";
                public const string Prefix = "package";
            }

            public static class Url
            {
                public const string Name = "url";
                public const string Template = "--url <url>";
                public const string HelpText = "Url to artifactory";
                public const string Prefix = "package";
                public const string DefaultValue = "https://packages.repo.dvint.de/";
            }
        }
    }
}
