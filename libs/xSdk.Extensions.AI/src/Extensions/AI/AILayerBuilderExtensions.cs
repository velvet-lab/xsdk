using CommunityToolkit.Diagnostics;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace xSdk.Extensions.AI;

public static class AILayerBuilderExtensions
{
    extension<TClient>(IAILayerBuilder<TClient> builder)
        where TClient : class
    {
        public IAILayerBuilder<TClient> RegisterChatClientFactory(Func<TClient, string, IChatClient> factory)
        {
            if (builder is AILayerBuilder<TClient> concreteLayer)
            {
                concreteLayer.AddChatClientFactories(factory);
            }

            return builder;
        }

        public IAILayerBuilder<TClient> RegisterAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory)
        {
            if (builder is AILayerBuilder<TClient> concreteLayer)
            {
                concreteLayer.AddAgentFactory(factory);
            }

            return builder;
        }

        public IAILayerBuilder<TClient> RegisterTool(string name, AIFunction aIFunction)
        {
            if (builder is AILayerBuilder<TClient> concreteLayer)
            {
                concreteLayer.AddTool(name, aIFunction);
            }

            return builder;
        }

        public IAILayerBuilder<TClient> AddAgentFile(string filePath)
        {
            Guard.IsNotNullOrEmpty(filePath);

            if (builder is AILayerBuilder<TClient> concreteLayer)
            {
                concreteLayer.AddDefinition(new AIDefinition()
                {
                    FilePath = filePath
                });
            }

            return builder;
        }
    }
}
