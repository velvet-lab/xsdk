using xSdk.Extensions.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting
{
    public abstract class PluginBase : PluginDescription, IPlugin
    {
        public virtual void ConfigureServices(IServiceCollection services) { }
    }
}
