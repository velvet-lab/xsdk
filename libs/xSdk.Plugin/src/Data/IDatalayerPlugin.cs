using xSdk.Extensions.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Data
{
    public interface IDatalayerPlugin : IPlugin
    {
        void ConfigureDatalayer(IServiceCollection services);
    }
}
