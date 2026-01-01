using xSdk.Extensions.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xSdk.Hosting
{
    public class HostPluginBase : PluginDescription, IPlugin
    {
        public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services) { }
    }
}
