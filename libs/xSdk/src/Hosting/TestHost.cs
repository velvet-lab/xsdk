using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class TestHost
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private const string APP_NAME = "xUnitTestHost";
    private const string APP_COMPANY = "xUnit";
    private const string APP_PREFIX = "UnitTest";

    public static IHostBuilder CreateBuilder() => CreateBuilder(new string[] { }, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, APP_COMPANY, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var slimHost = SlimHostInternal.InitializeTestHost(args, appName, appCompany, appPrefix);

        var builder = new HostBuilder()
            .ConfigureHostConfiguration(HostConfigurationManager.LoadTestConfiguration)
            .ConfigureServices(services =>
            {
                services
                    .AddLogging(HostLoggingManager.ConfigureSlimLogging)
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
            })
            .ConfigureWebHost(webhostBuilder =>
            {
                webhostBuilder.ConfigureServices(
                    (context, services) =>
                    {
                        SlimHostInternal.Instance.PluginSystem
                            .Invoke<WebHostPluginBase>(x => x.ConfigureServices(context, services));
                    }
                );
            });

        return builder;
    }
}
