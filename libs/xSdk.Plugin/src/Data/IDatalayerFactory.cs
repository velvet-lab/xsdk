using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Data
{
    public interface IDatalayerFactory
    {
        TRepository CreateRepository<TRepository>()
            where TRepository : IRepository => CreateRepository<TRepository>(null, null);

        TRepository CreateRepository<TRepository>(string name)
            where TRepository : IRepository;

        TRepository CreateRepository<TRepository>(IServiceScope scope)
            where TRepository : IRepository => CreateRepository<TRepository>(scope, null);

        TRepository CreateRepository<TRepository>(IServiceScope scope, string name)
            where TRepository : IRepository;
    }
}
