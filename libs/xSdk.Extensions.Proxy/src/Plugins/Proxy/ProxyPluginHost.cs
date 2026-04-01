using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Plugins.Proxy;

internal sealed class ProxyPluginHost : WebPluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddReverseProxy();
    }
}
