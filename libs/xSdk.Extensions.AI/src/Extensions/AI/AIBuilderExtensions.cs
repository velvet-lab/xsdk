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

using Microsoft.Extensions.Logging;
using xSdk.Extensions.AI;
using xSdk.Tools;

namespace xSdk.Plugins.AI;

public static class AIBuilderExtensions
{
    extension(AIBuilder builder)
    {
        public AIBuilder AddClient(string name, Action<ClientBuilder> configure)
        {
            var clientBuilder = builder.CreateBuilder<ClientBuilder>(typeof(ClientBuilder<>));
            clientBuilder.ConfigureBuilderAction = configure;
            clientBuilder.WithName(name);
            builder.ClientBuilders.AddOrNew(name, clientBuilder);

            return builder;
        }

        public AIBuilder AddTool(string name, Action<ToolBuilder> configure)
        {
            var toolBuilder = builder.CreateBuilder<ToolBuilder>(typeof(ToolBuilder<>));
            toolBuilder.ConfigureBuilderAction = configure;
            toolBuilder.WithName(name);
            builder.ToolBuilders.AddOrNew(name, toolBuilder);

            return builder;
        }

        public AIBuilder AddAgent(string name, Action<AgentBuilder> configure)
        {
            var agentBuilder = builder.CreateBuilder<AgentBuilder>(typeof(AgentBuilder<>));
            agentBuilder.ConfigureBuilderAction = configure;
            agentBuilder.WithName(name);
            builder.AgentBuilders.AddOrNew(name, agentBuilder);

            return builder;
        }

        public AIBuilder WithTelemetry(string sourceName = Diagnostics.SourceName, bool enableSensitiveData = false)
        {
            builder.Options.IsTelemetryEnabled = true;
            builder.DiagnosticsSourceName = sourceName;
            builder.Options.IsSensitiveDataEnabled = enableSensitiveData;

            return builder;
        }

        public AIBuilder WithLogging(ILoggerFactory factory)
        {
            builder.EnableLogging = true;
            builder.LoggerFactory = factory;
            return builder;
        }

        public AIBuilder ExposeOpenAIEndpoints()
        {
            builder.Options.ExposeOpenAIEndpoints = true;
            return builder;
        }

        public AIBuilder UseDevUI()
        {
            builder.Options.IsDevUiEnabled = true;
            return builder;
        }
    }
}
