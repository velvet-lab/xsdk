using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;
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
