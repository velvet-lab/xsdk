using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class Host
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var boot = SlimHostInternal.Initialize(args, appName, appCompany, appPrefix);

        var builder = new HostBuilder()
            .ConfigureHostConfiguration(HostConfigurationManager.LoadHostConfiguration)
            .ConfigureAppConfiguration(HostConfigurationManager.LoadAppConfiguration)
            .ConfigureServices(services =>
            {
                services
                    .AddLogging(HostLoggingManager.ConfigureLogging)
                    .AddFileServices()
                    .AddPluginServices()
                    .AddVariableServices();

                SlimHostInternal.Instance.PluginSystem
                    .Invoke<PluginBase>(x => x.ConfigureServices(services));
            })
            .ConfigureServices((context, services) =>
            {
                SlimHostInternal.Instance.PluginSystem
                    .Invoke<HostPluginBase>(x => x.ConfigureServices(context, services));
            });

        // Shutdown the logger
        AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
        {
            LogManager.Flush();
            LogManager.Shutdown();
        };

        return builder;
    }
}
