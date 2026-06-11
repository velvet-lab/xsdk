using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.AI;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

internal partial class AIPluginHost(IOptions<AIPluginOptions> pluginOptions, IOptions<EnvironmentOptions> environmentOptions, ILogger<AIPluginHost> logger) : WebPluginHost
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        IAIPluginBuilder? pluginBuilder = GetBuilder<IAIPluginBuilder>();

        if (pluginBuilder is not null)
        {
            logger.LogInformation("Configuring services for AgentsPluginHost with plugin pluginBuilder {PluginBuilderType}.", pluginBuilder.GetType().FullName);

            logger.LogDebug("Initializing plugin builder.");
            if (pluginBuilder is AIPluginBuilder concreatePluginBuilder)
            {
                pluginBuilder.Initialize();

                logger.LogDebug("Adding OpenAI responses, conversations, and DevUI services.");
                services
                    .AddOpenAIChatCompletions()
                    .AddOpenAIResponses()
                    .AddOpenAIConversations();

                if (environmentOptions.Value?.Stage == Stage.Development)
                {
                    logger.LogDebug("Development environment detected. Adding DevUI services.");
                    services.AddDevUI(options => options.AllowRemoteAccess = true);
                }

                concreatePluginBuilder.InitializeLayers(services, pluginOptions.Value, environmentOptions.Value);
            }
        }
        else
        {
            logger.LogError("Failed to get IAgentsPluginBuilder from plugin host. Ensure that the plugin is configured correctly.");
        }        
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder builder)
    {
        if (environmentOptions.Value?.Stage == Stage.Development)
        {
            logger.LogDebug("Development environment detected. Mapping DevUI endpoints.");
            builder.MapDevUI();
        }

        logger.LogDebug("Mapping OpenAI conversations and responses endpoints.");
        builder.MapOpenAIConversations();
        builder.MapOpenAIResponses();

        IAIPluginBuilder? pluginBuilder = GetBuilder<IAIPluginBuilder>();

        if (pluginBuilder is AIPluginBuilder concretePluginBuilder)
        {
            string[] agentKeys = concretePluginBuilder.GetRegisteredAgentKeys();
            foreach (string name in agentKeys)
            {
                IHostedAgentBuilder agentBuilder = builder.ServiceProvider.GetRequiredKeyedService<IHostedAgentBuilder>(name);
                builder.MapOpenAIChatCompletions(agentBuilder);
                builder.MapOpenAIResponses(agentBuilder);
            }
        }
    }
}
