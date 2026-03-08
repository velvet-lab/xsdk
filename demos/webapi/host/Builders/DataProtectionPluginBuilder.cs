using Microsoft.AspNetCore.DataProtection;
using xSdk.Extensions.Plugin;
using xSdk.Plugins.DataProtection;

namespace xSdk.Demos.Builders;

internal class DataProtectionPluginBuilder : PluginBuilderBase, IDataProtectionPluginBuilder
{
    public void ConfigureDataProtection(IDataProtectionBuilder builder) { }
}
