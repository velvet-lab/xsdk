using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Plugins.Agents;

internal class AgentsPluginHost : WebPluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        IMcpServerBuilder builder = services
            .AddMcpServer(options =>
            {
            
            });

        builder.
    }
}
