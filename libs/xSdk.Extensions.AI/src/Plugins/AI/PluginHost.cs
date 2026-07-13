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

using Microsoft.Agents.AI.DevUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

internal partial class PluginHost<TBuilder>(TBuilder builder, IOptions<EnvironmentOptions> environmentOptions, ILogger<PluginHost<TBuilder>> logger) : WebPluginHost
    where TBuilder : AIBuilder
{
    public override int Order => 20;
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        logger.LogInformation("Configuring services for AgentsPluginHost with plugin builder {PluginBuilderType}.", builder.GetType().FullName);

        logger.LogDebug("Initializing plugin builder.");
        builder.Build(services);

        if (builder.Options.ExposeOpenAIEndpoints)
        {
            if (builder.Options.IsDevUiEnabled && environmentOptions.Value?.Stage == Stage.Development)
            {
                logger.LogDebug("Development environment detected. Adding DevUI services.");
                services.AddDevUI(options => options.AllowRemoteAccess = true);
            }

            logger.LogDebug("Adding OpenAI responses, conversations, and DevUI services.");
            services
                .AddOpenAIChatCompletions()
                .AddOpenAIResponses()
                .AddOpenAIConversations();
        }
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder endpointBuilder)
    {
        if (builder.Options.ExposeOpenAIEndpoints)
        {
            if (builder.Options.IsDevUiEnabled && environmentOptions.Value?.Stage == Stage.Development)
            {
                logger.LogDebug("Development environment detected. Mapping DevUI endpoints.");
                endpointBuilder.MapDevUI();
            }

            logger.LogDebug("Mapping OpenAI conversations and responses endpoints.");
            endpointBuilder.MapOpenAIConversations();
            endpointBuilder.MapOpenAIResponses();
        }

        builder.ConfigureEndpoint(endpointBuilder);
    }
}
