/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
