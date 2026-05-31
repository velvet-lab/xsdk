using Microsoft.Agents.AI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Extensions.AI;
using xSdk.Extensions.Options;
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

internal class AIPluginHost(IOptions<AIPluginOptions> pluginOptions, IOptions<EnvironmentOptions> environmentOptions, IOptions<TelemetryPluginOptions> telemetryOptions, ILogger<AIPluginHost> logger) : WebPluginHost
{
    internal static Dictionary<string, AIAgentDefinition> Agents { get; } = new();

    public override void ConfigureServices(IServiceCollection services)
    {
        IAIPluginBuilder? pluginBuilder = this.GetBuilder<IAIPluginBuilder>();

        if (pluginBuilder is not null)
        {
            logger.LogInformation("Configuring services for AgentsPluginHost with plugin pluginBuilder {PluginBuilderType}.", pluginBuilder.GetType().FullName);

            logger.LogDebug("Initializing plugin builder.");
            pluginBuilder.Initialize();

            logger.LogDebug("Adding OpenAI responses, conversations, and DevUI services.");
            services
                .AddOpenAIChatCompletions()
                .AddOpenAIResponses()
                .AddOpenAIConversations();

            if(environmentOptions.Value?.Stage == Stage.Development)
            {
                logger.LogDebug("Development environment detected. Adding DevUI services.");
                services.AddDevUI(options =>
                {
                    options.AllowRemoteAccess = true;
                });
            }

            AddRegisteredAgents(services, pluginBuilder);
        }
        else
        {
            logger.LogError("Failed to get IAgentsPluginBuilder from plugin host. Ensure that the plugin is configured correctly.");
        }
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {

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

        foreach (string name in Agents.Keys)
        {
            IHostedAgentBuilder agentBuilder = builder.ServiceProvider.GetRequiredKeyedService<IHostedAgentBuilder>(name);
            builder.MapOpenAIChatCompletions(agentBuilder);
            builder.MapOpenAIResponses(agentBuilder);
        }
    }

    private void AddRegisteredAgents(IServiceCollection services, IAIPluginBuilder pluginBuilder)
    {
        logger.LogInformation("Loading agents into AIPluginHost.");

        logger.LogDebug("Creating default chat client for agents.");
        IChatClient defaultChatClient = pluginBuilder.CreateDefaultChatClient();

        if (defaultChatClient is null)
        {
            logger.LogError("Failed to create default chat client. Ensure that the plugin builder is configured correctly.");
            return;
        }

        // Register the default client with OTel for direct IChatClient injection.
        services.AddChatClient(
            defaultChatClient
                .AsBuilder()
                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = true)
                .Build());

        foreach ((string name, AIAgentDefinition agent) in Agents)
        {
            logger.LogInformation("Loaded agent: {AgentName}", name);
            agent.ChatClient ??= defaultChatClient;

            if (agent.ChatClient is null)
            {
                logger.LogError("Failed to create chat client for agent {AgentName}.", name);
                continue;
            }

            // Register the raw (non-wrapped) client as keyed service; OTel is applied once in the factory.
            services.AddKeyedChatClient(name, agent.ChatClient);

            IHostedAgentBuilder agentBuilder = services.AddAIAgent(name, (sp, key) =>
            {
                IChatClient chatClient = sp.GetRequiredKeyedService<IChatClient>(key)
                    .AsBuilder()
                    .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = true)
                    .Build();

                return new ChatClientAgent(chatClient, new ChatClientAgentOptions
                {
                    Name = agent.Name,
                    Description = agent.Description,
                    ChatOptions = new()
                    {
                        Instructions = agent.Instructions
                    }
                })
                .AsBuilder()
                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = true)
                .Build(sp);
            });

            services.AddKeyedSingleton(name, agentBuilder);
        }
    }
}
