using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Package
{
    public static class PluginExtensions
    {
        public static IPlugin CheckForUpdates(this IPlugin plugin)
        {
            //var updateSvc = host.Services.GetRequiredService<IUpdateService>();
            //if (updateSvc.CheckForUpdates())
            //{
            //    // Update is needed
            //}

            return plugin;
        }
    }
}
