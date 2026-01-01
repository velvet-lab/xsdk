using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Plugins.Telemetry
{
    internal class TelemetryPlugin : PluginBase
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var plugins = SlimHost.Instance.PluginSystem.GetPlugins();

            var telemetrySetup = SlimHost.Instance.VariableSystem.GetSetup<TelemetrySetup>();
            if (!telemetrySetup.IsDisabled)
            {
                telemetrySetup.Validate();

                // Create an builder
                var telemetryBuilder = services.AddOpenTelemetry();

                // Configure Tracing
                if (!telemetrySetup.IsTracingDisabled)
                {
                    telemetryBuilder.WithTracing(options =>
                    {
                        // Configure Exporter
                        options.ConfigureOtlpExporter(efPostSetup => { });

                        // Call tracing configuration from possible other Startups
                        SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureTracing(options));
                    });
                }

                // Configure Metrics
                if (!telemetrySetup.IsMetricsDisabled)
                {
                    telemetryBuilder.WithMetrics(options =>
                    {
                        // Configure Exporter
                        options.ConfigureOtlpExporter();

                        // Call metrics configuration from possible other Startups
                        SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureMetrics(options));
                    });
                }
            }

            // Configure Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                // Configure Default Logging
                // .ConfigureAutomationLogging()

                // Configure Exporter
                .ConfigureOtlpExporter(options =>
                {
                    // Call logging configuration from possible other Startups
                    SlimHost.Instance.PluginSystem.Invoke<ITelemetryPluginBuilder>(plugin => plugin.ConfigureLogging(options));
                });
            });
        }
    }
}
