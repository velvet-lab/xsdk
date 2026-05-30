using Microsoft.Extensions.AI;
using xSdk.Plugins.AI;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

public static class AIPluginBuilderExtensions
{
    public static IAIPluginBuilder RegisterAgent<TAgent>(this IAIPluginBuilder builder)
        where TAgent : AIAgentDefinition, new()
        => builder.RegisterAgentInternal<TAgent>(default);

    public static IAIPluginBuilder RegisterAgent<TAgent>(this IAIPluginBuilder builder, IChatClient? client)
        where TAgent : AIAgentDefinition, new()
        => builder.RegisterAgentInternal<TAgent>(client);

    private static IAIPluginBuilder RegisterAgentInternal<TAgent>(this IAIPluginBuilder builder, IChatClient? client)
        where TAgent : AIAgentDefinition, new()
    {
        TAgent agent = new()
        {
            ChatClient = client            
        };

        AIPluginHost.Agents.AddOrNew(agent.Name, agent);

        return builder;        
    }
}
