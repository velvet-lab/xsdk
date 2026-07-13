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

using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableTelemetry()
            => builder.EnableTelemetry(_ => { }, _ => { });

        public IHostBuilder EnableTelemetry(Action<TelemetryBuilder> configure)
            => builder.EnableTelemetry(configure, _ => { });

        public IHostBuilder EnableTelemetry(Action<TelemetryOptions> optionsConfigure)
            => builder.EnableTelemetry(_ => { }, optionsConfigure);

        public IHostBuilder EnableTelemetry(Action<TelemetryBuilder> configure, Action<TelemetryOptions> optionsConfigure)            
            => builder
                .RegisterPluginHost<PluginHost>()
                .RegisterPluginHostOptions<TelemetryOptions>(optionsConfigure)
                .RegisterBuilder<TelemetryBuilder>(configure);
    }
}
