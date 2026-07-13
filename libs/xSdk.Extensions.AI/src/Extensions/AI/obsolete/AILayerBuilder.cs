//using Microsoft.Extensions.AI;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using xSdk.Extensions.Logging;

//namespace xSdk.Extensions.AI;

//internal partial class AILayerBuilder(IServiceCollection services) : IAILayerBuilder    
//{
//    private static ILogger Logger => field ??= LogManager.CreateLogger<AILayerBuilder>();

//    private Dictionary<string, Func<IChatClient>> _clientFactories = [];

//    public IAILayerBuilder AddClientFactory(string? name, Func<IChatClient> factory)
//    {
//        _clientFactories[name ?? string.Empty] = factory;
//        return this;
//    }
//}
