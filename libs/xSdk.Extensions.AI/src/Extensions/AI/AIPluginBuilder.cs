using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

public abstract class AIPluginBuilder : PluginBuilder, IAIPluginBuilder
{
    private readonly Dictionary<Type, IAILayerBuilder> _aiLayerBuilders = [];    

    public abstract void Initialize();

    protected IAILayerBuilder<TClient> CreateAILayer<TClient>(Func<TClient> value)
        where TClient : class
    {
        Type key = typeof(TClient);
        if (_aiLayerBuilders.TryGetValue(key, out IAILayerBuilder? existingBuilder) && existingBuilder is IAILayerBuilder<TClient> typedBuilder)
        {
            return typedBuilder;
        }
        else
        {
            var aiLayer = new AILayerBuilder<TClient>(value);
            _aiLayerBuilders.AddOrNew(key, aiLayer);
            return aiLayer;
        }        
    }

    internal string[] GetRegisteredAgentKeys()
    {
        return [.. _aiLayerBuilders.Values.SelectMany(x => x.Definitions.Select(y => y.Name)) ];
    }

    internal void InitializeLayers(IServiceCollection services, AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
    {
        foreach (IAILayerBuilder builder in _aiLayerBuilders.Values)
        {
            builder.Build(services, pluginOptions, environmentOptions);
        }
    }
}
