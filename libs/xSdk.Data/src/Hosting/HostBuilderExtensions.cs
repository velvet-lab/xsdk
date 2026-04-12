using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using xSdk.Data;

namespace xSdk.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddDatalayer(this IHostBuilder hostBuilder, Action<IDatalayerBuilder> factory)
    {
        hostBuilder
            .ConfigureServices(services =>
            {
                var datalayerBuilder = new DatalayerBuilder(services);

                services
                
                    .AddSingleton<IDatalayerFactory>(provider =>
                    {
                        var factoryInstance = ActivatorUtilities.CreateInstance<DatalayerFactory>(provider);
                        return factoryInstance;
                    })
                    // Add pool provider for pooled database instances
                    .TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

                factory?.Invoke(datalayerBuilder);
            });
        
        return hostBuilder;
    }
}
