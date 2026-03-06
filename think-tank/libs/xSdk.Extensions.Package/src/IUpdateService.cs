using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Package
{
    public interface IUpdateService
    {
        bool CheckForUpdates<TComponent>()
            where TComponent : class => CheckForUpdatesAsync<TComponent>().GetAwaiter().GetResult();

        Task<bool> CheckForUpdatesAsync<TComponent>(CancellationToken token = default)
            where TComponent : class;
    }
}
