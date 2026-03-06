using xSdk.Extensions.Plugin;
using xSdk.Plugins.DataProtection;
using Microsoft.AspNetCore.DataProtection;

namespace xSdk.Demos.Builders
{
    internal class DataProtectionPluginBuilder : PluginBuilderBase, IDataProtectionPluginBuilder
    {
        public void ConfigureDataProtection(IDataProtectionBuilder builder) { }
    }
}
