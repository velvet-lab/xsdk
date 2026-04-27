using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting;

public interface IWebPluginHost : IPluginHost
{
    void ConfigureServices(WebHostBuilderContext context, IServiceCollection services);

    void ConfigureDefaults(WebHostBuilderContext context, IApplicationBuilder app);

    void Configure(WebHostBuilderContext context, IApplicationBuilder app);

    void ConfigureEndpoint(IEndpointRouteBuilder builder);
}
