using xSdk.Data;
using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableVaultAuth(this IHostBuilder builder)
        {
            builder
                .RegisterSetup<VaultSetup>()
                .RegisterSetup<AppRoleAuthSetup>()
                .RegisterSetup<CertAuthSetup>();

            return builder;
        }
    }
}
