using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

public class HostPluginBase : PluginDescription, IPlugin
{
    public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services) { }
}
