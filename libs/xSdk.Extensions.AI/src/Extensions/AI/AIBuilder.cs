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

using System.Runtime.CompilerServices;
using Microsoft.Agents.AI.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.AI;
using xSdk.Extensions.Builder;

namespace xSdk.Plugins.AI;

public class AIBuilder : BuilderBase
{
    private static AIBuilder? _instance;

    internal readonly Dictionary<string, ClientBuilder> ClientBuilders = new();
    internal readonly Dictionary<string, AgentBuilder> AgentBuilders = new();
    internal readonly Dictionary<string, ToolBuilder> ToolBuilders = new();
    internal readonly Dictionary<string, SkillBuilder> SkillBuilders = new();

    public AIBuilder()
    {
        _instance = this;
    }

    internal AIOptions Options => SlimServices.GetRequiredService<IOptions<AIOptions>>().Value;

    internal string? DiagnosticsSourceName { get; set; }

    internal ILoggerFactory? LoggerFactory { get; set; }

    internal bool EnableLogging { get; set; }

    internal static AIBuilder Instance => _instance ?? throw new InvalidOperationException("AIBuilder instance has not been initialized.");

    internal void Build(IServiceCollection services)
    {
        ConfigureBuilder();

        if (IsValid<AIBuilder, AIBuilderValidator>())
        {
            // Build all agents that have been configured
            foreach (var agentBuilder in AgentBuilders.Values)
            {
                agentBuilder.Build(services);
            }
        }
    }

    internal void ConfigureEndpoint(IEndpointRouteBuilder builder)
    {
        if (Options.ExposeOpenAIEndpoints)
        {
            foreach (var agentName in AgentBuilders.Values.Select(x => x.Name))
            {
                IHostedAgentBuilder hostedAgentBuilder = builder.ServiceProvider.GetRequiredKeyedService<IHostedAgentBuilder>((object?)agentName);
                builder.MapOpenAIChatCompletions(hostedAgentBuilder);
                builder.MapOpenAIResponses(hostedAgentBuilder);
            }
        }
    }

    protected AIBuilder CreateBuilder()
    {
        return this;
    }

    internal TBuilder CreateBuilder<TBuilder>(Type type)
        where TBuilder : class
    {
        Type genericType = type.MakeGenericType(new Type[] { this.GetType() });
        TBuilder concreteBuilder = (SlimServices.GetRequiredService(genericType) as TBuilder)!;
        return concreteBuilder;
    }
}
