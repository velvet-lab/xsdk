using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

internal class IngestionPluginHost() : PluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        throw new NotSupportedException();
    }
}
