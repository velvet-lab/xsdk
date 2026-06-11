using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.AI;

internal partial class AILayerBuilder<TClient>(Func<TClient> clientFactory) : IAILayerBuilder<TClient>
    where TClient : class
{
    private static ILogger<AILayerBuilder<TClient>>? _logger;
    private static ILogger Logger => _logger ??= LogManager.CreateLogger<AILayerBuilder<TClient>>();
}
