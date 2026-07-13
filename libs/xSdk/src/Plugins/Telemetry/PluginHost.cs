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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Logs;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;

namespace xSdk.Plugins.Telemetry;

internal sealed class PluginHost(TelemetryBuilder builder) : PluginHostBase
{
    public override void ConfigureLogging(ILogBuilder builder)
    {
        // Allow all logging for OpenTelemetryLoggerProvider, as filtering is done in OpenTelemetryOptions.
        builder.IsLoggingAllowed<OpenTelemetryLoggerProvider>(level => true);
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        builder.Build(services);
    }
}
