using xSdk.Extensions.Plugin;

namespace xSdk.Data
{
    public interface IDataMigrationPlugin : IPlugin
    {
        void EnsureDataMigration();

        void Initialize(IServiceProvider provider);

        // void ConfigureServices(IServiceCollection services);
    }
}
