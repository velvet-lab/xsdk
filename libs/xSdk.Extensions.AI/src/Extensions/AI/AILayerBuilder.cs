using CommunityToolkit.Diagnostics;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Agents.ObjectModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Options;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

internal class AILayerBuilder<TClient>(Func<TClient> clientFactory) : IAILayerBuilder<TClient>
    where TClient : class
{
    private readonly ILogger _logger = LogManager.CreateLogger<AILayerBuilder<TClient>>();

    private readonly Dictionary<string, IChatClient> _chatClients = [];
    private readonly List<AIDefinition> _definitions = [];

    private Func<TClient, string, IChatClient>? _chatClientFactory;
    private Func<IServiceProvider, string, AIDefinition, AIAgent?>? _agentFactory;

    public IList<AIDefinition> Definitions => _definitions;

    internal void AddChatClientFactories(Func<TClient, string, IChatClient> factory)
        => _chatClientFactory = factory;

    internal void AddAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory)
        => _agentFactory = factory;

    internal void AddDefinition(AIDefinition definition)
        => _definitions.Add(definition);

    public void Build(IServiceCollection services, AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
    {
        _logger.LogInformation("Initializing AI Layer for client type: {ClientType}", typeof(TClient).Name);

        TClient client = clientFactory.Invoke();

        foreach (AIDefinition definition in _definitions)
        {
            if (definition.TryReadMetadata(pluginOptions, environmentOptions, out GptComponentMetadata? metadata))
            {
                definition.Metadata = metadata;
            }            

            _logger.LogInformation("Loaded definition: {AgentName}", definition.Name);

            string? modelName = definition.Model ?? pluginOptions?.Model;
            IChatClient chatClient = RetrieveChatClients(client, modelName);
            services.AddKeyedChatClient(definition.Name, chatClient);

            if (_agentFactory is not null)
            {
                IHostedAgentBuilder agentBuilder = services.AddAIAgent(definition.Name, (provider, key) =>
                {
                    AIAgent? realAgent = _agentFactory.Invoke(provider, key, definition);
                    return realAgent ?? throw new InvalidOperationException("Failed to create definition instance for definition: " + definition.Name);
                });
                services.AddKeyedSingleton(definition.Name, agentBuilder);
            }
        }
    }

    private IChatClient RetrieveChatClients(TClient client, string? model)
    {
        Guard.IsNotNull(model);

        if (!_chatClients.ContainsKey(model))
        {            
            // Build chat client here
            IChatClient chatClient = _chatClientFactory?.Invoke(client, model) ?? throw new InvalidOperationException("Failed to create chat client.");
            _chatClients.AddOrNew(model, chatClient);
        }

        return _chatClients[model];
    }
}
