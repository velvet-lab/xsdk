using xSdk.Extensions.Plugin;
using Microsoft.AspNetCore.DataProtection;

namespace xSdk.Plugins.DataProtection
{
    public interface IDataProtectionPluginBuilder : IPluginBuilder
    {
        void ConfigureDataProtection(IDataProtectionBuilder builder);
    }
}
