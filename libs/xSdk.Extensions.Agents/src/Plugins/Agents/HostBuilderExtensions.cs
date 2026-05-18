using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

namespace xSdk.Plugins.Agents;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableLinks<TPluginBuilder>(this IHostBuilder hostBuilder)
        where TPluginBuilder : class//, ILinksPluginBuilder
    {
        return hostBuilder
            .RegisterPluginHost<AgentsPluginHost>();
        //.RegisterPluginBuilder<ILinksPluginBuilder, TPluginBuilder>();
    }
}
