using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Hosting;

namespace xSdk.Extensions.Package.Providers.Local
{
    public class LocalSetup : Setup
    {
        [Variable(name: Definitions.Path.Name, template: Definitions.Path.Template, helpText: Definitions.Path.HelpText, prefix: Definitions.Path.Prefix)]
        public string Path
        {
            get => this.ReadValue<string>(Definitions.Path.Name);
            set => this.SetValue(Definitions.Path.Name, value);
        }

        protected override void Initialize()
        {
            if (string.IsNullOrEmpty(Path))
            {
                Path = SlimHost.Instance.FileSystem.User.Data.GetFullPath("/cache");
            }
        }

        public static class Definitions
        {
            public static class Path
            {
                public const string Name = "path";
                public const string Template = "--path <cache>";
                public const string HelpText = "Location for the package";
                public const string Prefix = "local";
            }
        }
    }
}
