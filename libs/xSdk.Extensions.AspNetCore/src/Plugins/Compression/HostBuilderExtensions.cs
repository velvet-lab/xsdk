using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.Compression
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableCompression(this IHostBuilder hostBuilder)
        {
            return hostBuilder.EnablePlugin<CompressionPlugin>();
        }
    }
}
