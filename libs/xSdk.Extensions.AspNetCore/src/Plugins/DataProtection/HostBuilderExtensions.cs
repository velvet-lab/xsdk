using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.DataProtection
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableDataProtection(this IHostBuilder hostBuilder)
        {
            return hostBuilder.RegisterSetup<DataProtectionSetup>().EnablePlugin<DataProtectionPlugin>();
        }

        public static IHostBuilder EnableDataProtection<TPluginBuilder>(this IHostBuilder hostBuilder)
            where TPluginBuilder : IDataProtectionPluginBuilder
        {
            return hostBuilder.EnableDataProtection().EnablePlugin<TPluginBuilder>();
        }
    }
}
