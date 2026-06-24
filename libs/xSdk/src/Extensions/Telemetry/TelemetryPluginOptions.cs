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

using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Telemetry;

public sealed class TelemetryPluginOptions : PluginOptions
{
    [Variable(
        name: Definitions.LoggingEnabled.Name,
        template: Definitions.LoggingEnabled.Template,
        helpText: Definitions.LoggingEnabled.HelpText        
    )]
    public bool LoggingEnabled
    {
        get => ReadValue<bool>(Definitions.LoggingEnabled.Name);
        set => SetValue(Definitions.LoggingEnabled.Name, value);
    }

    [Variable(
        name: Definitions.TracingEnabled.Name,
        template: Definitions.TracingEnabled.Template,
        helpText: Definitions.TracingEnabled.HelpText       
    )]
    public bool TracingEnabled
    {
        get => ReadValue<bool>(Definitions.TracingEnabled.Name);
        set => SetValue(Definitions.TracingEnabled.Name, value);
    }

    [Variable(
        name: Definitions.MetricsEnabled.Name,
        template: Definitions.MetricsEnabled.Template,
        helpText: Definitions.MetricsEnabled.HelpText        
    )]
    public bool MetricsEnabled
    {
        get => ReadValue<bool>(Definitions.MetricsEnabled.Name);
        set => SetValue(Definitions.MetricsEnabled.Name, value);
    }

    internal static class Definitions
    {
        public static class LoggingEnabled
        {
            public const string Name = nameof(LoggingEnabled);
            public const string Template = "--enable-logging";
            public const string HelpText = "Enables logging telemetry.";
        }

        public static class TracingEnabled
        {
            public const string Name = nameof(TracingEnabled);
            public const string Template = "--enable-tracing";
            public const string HelpText = "Enables tracing telemetry.";
        }

        public static class MetricsEnabled
        {
            public const string Name = nameof(MetricsEnabled);
            public const string Template = "--enable-metrics";
            public const string HelpText = "Enables metrics telemetry.";
        }
    }
}
