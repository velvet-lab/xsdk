using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Telemetry
{
    public sealed class TelemetrySetup : Setup
    {
        [Variable(name: Definitions.DisableAll.Name, template: Definitions.DisableAll.Template, helpText: Definitions.DisableAll.HelpText)]
        public bool IsDisabled
        {
            get => this.ReadValue<bool>(Definitions.DisableAll.Name);
            set => this.SetValue(Definitions.DisableAll.Name, value);
        }

        [Variable(name: Definitions.LoggingDisabled.Name, template: Definitions.LoggingDisabled.Template, helpText: Definitions.LoggingDisabled.HelpText)]
        public bool IsLoggingDisabled
        {
            get => this.ReadValue<bool>(Definitions.LoggingDisabled.Name);
            set => this.SetValue(Definitions.LoggingDisabled.Name, value);
        }

        [Variable(name: Definitions.TracingDisabled.Name, template: Definitions.TracingDisabled.Template, helpText: Definitions.TracingDisabled.HelpText)]
        public bool IsTracingDisabled
        {
            get => this.ReadValue<bool>(Definitions.TracingDisabled.Name);
            set => this.SetValue(Definitions.TracingDisabled.Name, value);
        }

        [Variable(name: Definitions.MetricsDisabled.Name, template: Definitions.MetricsDisabled.Template, helpText: Definitions.MetricsDisabled.HelpText)]
        public bool IsMetricsDisabled
        {
            get => this.ReadValue<bool>(Definitions.MetricsDisabled.Name);
            set => this.SetValue(Definitions.MetricsDisabled.Name, value);
        }

        [Variable(
            name: Definitions.OtlpExporterDisabled.Name,
            template: Definitions.OtlpExporterDisabled.Template,
            helpText: Definitions.OtlpExporterDisabled.HelpText,
            defaultValue: Definitions.OtlpExporterDisabled.DefaultValue
        )]
        public bool IsOtlpExporterDisabled
        {
            get => this.ReadValue<bool>(Definitions.OtlpExporterDisabled.Name);
            set => this.SetValue(Definitions.OtlpExporterDisabled.Name, value);
        }

        [Variable(
            name: Definitions.ConsoleEnabled.Name,
            template: Definitions.ConsoleEnabled.Template,
            helpText: Definitions.ConsoleEnabled.HelpText,
            defaultValue: Definitions.ConsoleEnabled.DefaultValue
        )]
        public bool IsConsoleEnabled
        {
            get => this.ReadValue<bool>(Definitions.ConsoleEnabled.Name);
            set => this.SetValue(Definitions.ConsoleEnabled.Name, value);
        }

        [Variable(name: Definitions.Token.Name, template: Definitions.Token.Template, helpText: Definitions.Token.HelpText)]
        public string Token
        {
            get => this.ReadValue<string>(Definitions.Token.Name);
            set => this.SetValue(Definitions.Token.Name, value);
        }

        [Variable(
            name: Definitions.Endpoint.Name,
            template: Definitions.Endpoint.Template,
            helpText: Definitions.Endpoint.HelpText,
            defaultValue: Definitions.Endpoint.DefaultValue
        )]
        public string Endpoint
        {
            get => this.ReadValue<string>(Definitions.Endpoint.Name);
            set => this.SetValue(Definitions.Endpoint.Name, value);
        }

        [Variable(
            name: Definitions.LogLevel.Name,
            template: Definitions.LogLevel.Template,
            helpText: Definitions.LogLevel.HelpText,
            defaultValue: Definitions.LogLevel.DefaultValue
        )]
        public string LogLevel
        {
            get => this.ReadValue<string>(Definitions.LogLevel.Name);
            set => this.SetValue(Definitions.LogLevel.Name, value);
        }

        [Variable(
            name: Definitions.SdkLogLevel.Name,
            template: Definitions.SdkLogLevel.Template,
            helpText: Definitions.SdkLogLevel.HelpText,
            defaultValue: Definitions.SdkLogLevel.DefaultValue
        )]
        public string SdkLogLevel
        {
            get => this.ReadValue<string>(Definitions.SdkLogLevel.Name);
            set => this.SetValue(Definitions.SdkLogLevel.Name, value);
        }

        [Variable(
            name: Definitions.OtlpLogLevel.Name,
            template: Definitions.OtlpLogLevel.Template,
            helpText: Definitions.OtlpLogLevel.HelpText,
            defaultValue: Definitions.OtlpLogLevel.DefaultValue
        )]
        public string OtlpLogLevel
        {
            get => this.ReadValue<string>(Definitions.OtlpLogLevel.Name);
            set => this.SetValue(Definitions.OtlpLogLevel.Name, value);
        }

        public ITelemetryPluginBuilder Configuration { get; set; }

        protected override void ValidateSetup()
        {
            this.ValidateMember(
                x => !x.IsDisabled && !x.IsOtlpExporterDisabled && string.IsNullOrEmpty(x.Token),
                "No token given to authenticate against MaaS endpoint",
                Definitions.Token.Name
            );

            this.ValidateMember(
                x => !x.IsDisabled && !x.IsOtlpExporterDisabled && string.IsNullOrEmpty(x.Endpoint),
                "No MaaS endpoint configured",
                Definitions.Endpoint.Name
            );

            if (!string.IsNullOrEmpty(Token) && !string.IsNullOrEmpty(Endpoint))
            {
                IsOtlpExporterDisabled = false;
            }
        }

        public static class Definitions
        {
            public static class DisableAll
            {
                public const string Name = "disable-all";
                public const string Template = "--disable-all";
                public const string HelpText = "Telemetry will disabled. No logging, no tracing, no metrics!";
            }

            public static class LoggingDisabled
            {
                public const string Name = "disable-logging";
                public const string Template = "--disable-logging";
                public const string HelpText = "Only logging will disabled. Tracing and metrics will remain active";
            }

            public static class TracingDisabled
            {
                public const string Name = "disable-tracing";
                public const string Template = "--disable-tracing";
                public const string HelpText = "Only tracing will disabled. Logging and metrics will remain active";
            }

            public static class MetricsDisabled
            {
                public const string Name = "disable-metrics";
                public const string Template = "--disable-metrics";
                public const string HelpText = "Only metrics will disabled. Logging and tracing will remain active";
            }

            public static class OtlpExporterDisabled
            {
                public const string Name = "disable-otlp-exporter";
                public const string Template = "--disable-otlp-exporter";
                public const string HelpText =
                    "Open telemetry exporter will disabled. Only console logging will remain active. No metrics or tracing available.";
                public const bool DefaultValue = true;
            }

            public static class OtlpLogLevel
            {
                public const string Name = "log-level-otlp";
                public const string Template = "--log-level-otlp <level>";
                public const string HelpText = "Log Level for Monitoring as a Service";
                public const string DefaultValue = "Information";
            }

            public static class ConsoleEnabled
            {
                public const string Name = "enable-console";
                public const string Template = "--enable-console";
                public const string HelpText = "Enables Console Output";
                public const bool DefaultValue = false;
            }

            public static class Token
            {
                public const string Name = "maas-token";
                public const string Template = "--maas-token <token>";
                public const string HelpText = "Token to authenticate the application in MaaS environments";
            }

            public static class Endpoint
            {
                public const string Name = "maas-endpoint";
                public const string Template = "--maas-endpoint <endpoint>";
                public const string HelpText = "gRpc Endpoit where MaaS lives.";
                public const string DefaultValue = "https://otel-collector.monitoring.dvint.de:4317";
            }

            public static class LogLevel
            {
                public const string Name = "log-level";
                public const string Template = "--log-level <level>";
                public const string HelpText = "Log Level for Automation";
                public const string DefaultValue = "None";
            }

            public static class SdkLogLevel
            {
                public const string Name = "log-level-sdk";
                public const string Template = "--log-level-sdk <level>";
                public const string HelpText = "Log Level for Automation and all others";
                public const string DefaultValue = "None";
            }
        }
    }
}
