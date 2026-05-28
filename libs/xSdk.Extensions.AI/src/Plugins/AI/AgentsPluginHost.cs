using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Extensions.AI;
using xSdk.Extensions.AI.Agents;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

internal class AgentsPluginHost(IOptions<EnvironmentOptions> environmentOptions, IOptions<AgentsPluginOptions> pluginOptions, ILogger<AgentsPluginHost> logger) : WebPluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        IAgentsPluginBuilder? pluginBuilder = this.GetBuilder<IAgentsPluginBuilder>();

        if (pluginBuilder is not null)
        {
            logger.LogInformation("Configuring services for AgentsPluginHost with plugin builder {PluginBuilderType}.", pluginBuilder.GetType().FullName);
            logger.LogDebug("Configuring services for AgentsPluginHost.");
            services
                .AddSingleton<IAgentService, AgentService>();

            logger.LogDebug("Creating chat client from plugin builder.");
            IChatClient chatClient = pluginBuilder.CreateChatClient();
            services
                .AddChatClient(chatClient);

            logger.LogDebug("Adding OpenAI responses, conversations, and DevUI services.");
            services                
                .AddOpenAIResponses()
                .AddOpenAIConversations();

            if(environmentOptions.Value?.Stage == Stage.Development)
            {
                logger.LogDebug("Development environment detected. Adding DevUI services.");
                services.AddDevUI();
            }

            logger.LogDebug("Adding AIAgent services with in-memory session store.");
            var agentBuilder = services
                .AddAIAgent("Assistant", (sp, name) =>
                {
                    var agentService = sp.GetRequiredService<IAgentService>();
                    return agentService.CreateAgentAsync().GetAwaiter().GetResult();
                })
                .WithInMemorySessionStore();
            services.AddKeyedSingleton<IHostedAgentBuilder>("Assistant", agentBuilder);
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

        var agent = builder.ServiceProvider.GetRequiredKeyedService<IHostedAgentBuilder>("Assistant");

        builder.MapOpenAIChatCompletions(agent);

        builder.MapOpenAIResponses(agent);
        builder.MapOpenAIConversations();
        
    }
}
