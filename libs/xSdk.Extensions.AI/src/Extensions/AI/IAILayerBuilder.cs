using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.AI;

public interface IAILayerBuilder
{
    IList<AIDefinition> Definitions { get; }

    void Build(IServiceCollection services, AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions);
}

public interface IAILayerBuilder<TClient> : IAILayerBuilder
    where TClient : class
{
}
