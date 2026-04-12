using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Data;

internal class DatabaseBuilder(string name, IServiceCollection services) : IDatabaseBuilder
{

    public IDatabaseBuilder ConfigureRepository<TImplementation>()
        where TImplementation : class, IRepository
    {
        services.TryAddKeyedScoped(name, (provider, key) =>
        {
            TImplementation instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
            if (instance is Repository repository)
            {
                repository.DatalayerName = (string)key;
            }
            return instance;
        });

        return this;
    }

    public IDatabaseBuilder ConfigureRepository<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, IRepository, TInterface
    {
        services.TryAddKeyedScoped<TInterface>(name, (provider, key) =>
        {
            TImplementation instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
            if (instance is Repository repository)
            {
                repository.DatalayerName = (string)key;
            }

            return instance;
        });

        return this;
    }
}
