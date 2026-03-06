using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xSdk.Extensions.Package
{
    public static class HostExtensions
    {
        public static IHost CheckForUpdates<TComponent>(this IHost host)
            where TComponent : class
        {
            var updateSvc = host.Services.GetRequiredService<IUpdateService>();
            if (updateSvc.CheckForUpdates<TComponent>())
            {
                // Update is needed
            }

            return host;
        }
    }
}
