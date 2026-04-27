using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.IO;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Hosting.Managers;

namespace xSdk.Hosting;

public static class TestHostFactory
{
    public static IHostBuilder CreateTestHost(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        ApplicationOptions appOptions = new()
        {
            Name = appName ?? ApplicationOptions.Definitions.AppName.DefaultValue,
            Company = appCompany ?? ApplicationOptions.Definitions.AppCompany.DefaultValue,
            Prefix = appPrefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue
        };

        var slimHost = SlimHost.InitializeSlimHost(args, appOptions);

        // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden
#pragma warning disable EXTEXP0016
        var builder = Microsoft.Extensions.Hosting.Testing.FakeHost.CreateBuilder(config =>
        {
            config.FakeLogging = true;
            config.FakeRedaction = true;
        });
#pragma warning restore EXTEXP0016

        builder
            .SetSlimHost(slimHost)
            .ConfigureHostConfiguration(HostConfigurationManager.LoadTestConfiguration)
            .ConfigureServices(services =>
            {
                slimHost.PostConfigure(services);

                services
                    .RegisterApplicationOptions(appOptions)
                    .RegisterOptions<EnvironmentOptions>()
                    .AddLogging()
                    .AddVariableServices()
                    .AddFileServices()
                    .AddPluginServices();

                // Add initializer for Logger Factory
                services
                    .AddHostedService<HostInitializer>();

                slimHost.ConfigurePluginHost(x => x.ConfigureServices(services));
            })
            .ConfigureServices((context, services) =>
            {
                slimHost.ConfigurePluginHost(x => x.ConfigureServices(context, services));
            });

        return builder;
    }
}
