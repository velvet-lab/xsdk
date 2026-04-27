using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using xSdk.Plugins.WebApi;

namespace xSdk.Hosting;

public static partial class TestWebHost
{
    private const string APP_NAME = "xUnitWebTestHost";
    private const string APP_COMPANY = "xUnit";
    private const string APP_PREFIX = "UnitTest";

    public static IHostBuilder CreateBuilder() => CreateBuilder(new string[] { }, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, APP_COMPANY, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var builder = TestHostFactory.CreateTestHost(args, appName, appCompany, appPrefix);
        var slimHost = builder.GetSlimHost();

        builder.ConfigureWebHost(webhostBuilder =>
        {
#pragma warning disable EXTEXP0014 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
            webhostBuilder
                .UseFakeStartup()
                .ListenHttpOnAnyPort();
#pragma warning restore EXTEXP0014 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.

            webhostBuilder
                .ConfigureServices(services =>
                {
                    slimHost.ConfigureWebPluginHost(x => x.ConfigureServices(services));
                })
                .ConfigureServices((context, services) =>
                {
                    slimHost.ConfigureWebPluginHost(x => x.ConfigureServices(context, services));
                });
        });

        return builder;
    }
}
