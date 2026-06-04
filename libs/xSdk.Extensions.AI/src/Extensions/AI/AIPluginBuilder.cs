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
    private readonly Dictionary<Type, IAILayerBuilder> _aiLayers = [];    

    public abstract void Initialize();

    protected IAILayerBuilder<TClient> CreateAILayer<TClient>(Func<TClient> value)
        where TClient : class
    {
        Type key = typeof(TClient);
        if (_aiLayers.TryGetValue(key, out IAILayerBuilder? existingLayer) && existingLayer is IAILayerBuilder<TClient> typedLayer)
        {
            return typedLayer;
        }
        else
        {
            var aiLayer = new AILayerBuilder<TClient>(value);
            _aiLayers.AddOrNew(key, aiLayer);
            return aiLayer;
        }        
    }

    internal string[] GetRegisteredAgentKeys()
    {
        return [.. _aiLayers.Values.SelectMany(x => x.Definitions.Select(y => y.Name))];
    }

    internal void InitializeLayers(IServiceCollection services, AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
    {
        foreach (IAILayerBuilder layer in _aiLayers.Values)
        {
            layer.Build(services, pluginOptions, environmentOptions);
        }
    }
}
