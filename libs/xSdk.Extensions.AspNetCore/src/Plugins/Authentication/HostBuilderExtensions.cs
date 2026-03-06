using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.Authentication
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableAuthentication(this IHostBuilder hostBuilder)
        {
            hostBuilder.RegisterSetup<ApiKeySetup>().EnablePlugin<AuthenticationPlugin>();

            return hostBuilder;
        }

        public static IHostBuilder EnableAuthentication<TPluginBuilder>(this IHostBuilder hostBuilder)
            where TPluginBuilder : IAuthenticationPluginBuilder
        {
            hostBuilder.EnableAuthentication().EnablePlugin<TPluginBuilder>();

            return hostBuilder;
        }
    }
}
