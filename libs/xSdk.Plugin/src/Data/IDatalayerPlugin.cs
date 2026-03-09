using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Data;

public interface IDatalayerPlugin : IPlugin
{
    void ConfigureDatalayer(IServiceCollection services);
}
