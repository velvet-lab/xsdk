using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Telemetry;

public static class TelemetryBuilderExtensions
{
    extension(TelemetryBuilder builder)
    {
        public TelemetryBuilder WithResources(Action<ResourceBuilder> configure)
        {
            builder
                .AsBuilder<TelemetryBuilder>()
                .ResourceBuilderDelegate = configure;

            return builder;
        }

        public TelemetryBuilder WithLogging(Action<LoggerProviderBuilder> configure)
            => builder.WithLogging(configure, null);

        public TelemetryBuilder WithLogging(Action<LoggerProviderBuilder> configure, Action<OpenTelemetryLoggerOptions>? options)
        {
            TelemetryBuilder concreteBuilder = builder
                .AsBuilder<TelemetryBuilder>();

            concreteBuilder.ConfigureLoggingDelegate = configure;
            if (options is not null)
            {
                concreteBuilder.ConfigureLoggingOptionsDelegate = options;
            }

            return builder;
        }

        public TelemetryBuilder WithMetrics(Action<MeterProviderBuilder> configure)
        {
            builder
                .AsBuilder<TelemetryBuilder>()
                .ConfigureMetricsDelegate = configure;

            return builder;
        }

        public TelemetryBuilder WithTracing(Action<TracerProviderBuilder> configure)
        {
            builder
                .AsBuilder<TelemetryBuilder>()
                .ConfigureTracingDelegate = configure;

            return builder;
        }

        public TelemetryBuilder WithLoggingEnabled()
        {
            builder
                .Options.LoggingEnabled = true;

            return builder;
        }

        public TelemetryBuilder WithMetricsEnabled()
        {
            builder
                .Options.MetricsEnabled = true;

            return builder;
        }

        public TelemetryBuilder WithTracingEnabled()
        {
            builder
                .Options.TracingEnabled = true;
            return builder;
        }
    }
}
