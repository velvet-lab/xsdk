using Microsoft.AspNetCore.DataProtection;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.DataProtection;

public interface IDataProtectionPluginBuilder : IPluginBuilder
{
    void ConfigureDataProtection(IDataProtectionBuilder builder);
}
