using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Hosting
{
    public sealed class WebHostSetup : Setup, IWebHostSetup
    {
        [Variable(
            name: Definitions.Bind.Name,
            template: Definitions.Bind.Template,
            helpText: Definitions.Bind.HelpText,
            defaultValue: Definitions.Bind.DefaultValue
        )]
        public string Bind
        {
            get => ReadValue<string>(Definitions.Bind.Name);
            set => SetValue(Definitions.Bind.Name, value);
        }

        [Variable(
            name: Definitions.Http.Name,
            template: Definitions.Http.Template,
            helpText: Definitions.Http.HelpText,
            defaultValue: Definitions.Http.DefaultValue
        )]
        public int Http
        {
            get => ReadValue<int>(Definitions.Http.Name);
            set => SetValue(Definitions.Http.Name, value);
        }

        [Variable(name: Definitions.Grpc.Name, template: Definitions.Grpc.Template, helpText: Definitions.Grpc.HelpText)]
        public int Grpc
        {
            get => ReadValue<int>(Definitions.Grpc.Name);
            set => SetValue(Definitions.Grpc.Name, value);
        }

        [Variable(
            name: Definitions.Https.Name,
            template: Definitions.Https.Template,
            helpText: Definitions.Https.HelpText,
            defaultValue: Definitions.Https.DefaultValue
        )]
        public int Https
        {
            get => ReadValue<int>(Definitions.Https.Name);
            set => SetValue(Definitions.Https.Name, value);
        }

        [Variable(name: Definitions.TlsCertFile.Name, template: Definitions.TlsCertFile.Template, helpText: Definitions.TlsCertFile.HelpText)]
        public string TlsCertFile
        {
            get => ReadValue<string>(Definitions.TlsCertFile.Name);
            set => SetValue(Definitions.TlsCertFile.Name, value);
        }

        [Variable(name: Definitions.TlsKeyFile.Name, template: Definitions.TlsKeyFile.Template, helpText: Definitions.TlsKeyFile.HelpText)]
        public string TlsKeyFile
        {
            get => ReadValue<string>(Definitions.TlsKeyFile.Name);
            set => SetValue(Definitions.TlsKeyFile.Name, value);
        }

        [Variable(name: Definitions.AllowSystemPorts.Name, template: Definitions.AllowSystemPorts.Template, helpText: Definitions.AllowSystemPorts.HelpText)]
        public bool AllowSystemPorts
        {
            get => ReadValue<bool>(Definitions.AllowSystemPorts.Name);
            set => SetValue(Definitions.AllowSystemPorts.Name, value);
        }

        public bool IsHttpsEnabled { get; private set; }

        protected override void Initialize()
        {
            if (Https > 0)
            {
                if (!string.IsNullOrEmpty(TlsCertFile) && !string.IsNullOrEmpty(TlsKeyFile))
                {
                    IsHttpsEnabled = true;
                }
            }
        }

        public static class Definitions
        {
            public static class Bind
            {
                public const string Name = "bind";
                public const string Template = "--bind <host>";
                public const string HelpText = "Starts as Server and binds to given host. Default is 'localhost'"; // DevSkim: ignore DS162092
                public const string DefaultValue = "localhost"; // DevSkim: ignore DS162092
            }

            public static class Http
            {
                public const string Name = "http";
                public const string Template = "--http <port>";
                public const string HelpText = "Starts as Server to listen for Http Traffic on given Port";
                public const int DefaultValue = 8080;
            }

            public static class Https
            {
                public const string Name = "https";
                public const string Template = "--https <port>";
                public const string HelpText = "Starts as Server to listen for Https Traffic on given Port";
                public const int DefaultValue = 8081;
            }

            public static class Grpc
            {
                public const string Name = "grpc";
                public const string Template = "--grpc <port>";
                public const string HelpText = "Starts as Server to listen for Grpc Traffic on given Port";
            }

            public static class TlsCertFile
            {
                public const string Name = "tls-cert-file";
                public const string Template = "--tls-cert-file <file>";
                public const string HelpText = "Specifies the path to the certificate for TLS. It requires a PEM-encoded file";
            }

            public static class TlsKeyFile
            {
                public const string Name = "tls-key-file";
                public const string Template = "--tls-key-file <file>";
                public const string HelpText = "Specifies the path to the private key for the certificate. It requires a PEM-encoded file";
            }

            public static class AllowSystemPorts
            {
                public const string Name = "allow-system-ports";
                public const string Template = "--allow-system-ports";
                public const string HelpText = "System Ports (lower 1024) are allowed";
            }
        }
    }
}
