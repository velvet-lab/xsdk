using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

internal sealed class SlimHostInternal : SlimHostBase
{
    private static ISlimHost host;

    internal static ISlimHost Instance => host ?? throw new InvalidOperationException("SlimHost is not initialized");

    internal static ISlimHost Initialize(string[] args, string? appName, string? appCompany, string? appPrefix) =>
        InitializeSlimHost(args, appName, appCompany, appPrefix, false);

    internal static ISlimHost InitializeTestHost(string[] args, string? appName, string? appCompany, string? appPrefix) =>
        InitializeSlimHost(args, appName, appCompany, appPrefix, true);

    private static ISlimHost InitializeSlimHost(string[] args, string? appName, string? appCompany, string? appPrefix, bool isTestHost)
    {
        if (host == null)
        {
            HostLoggingManager.ResetLogger();

            var builder = SlimHostBuilder
                .CreateBuilder<SlimHostInternal>()
                .ValidateAppName(appName, EnvironmentSetup.Definitions.AppName.DefaultValue)
                .ValidateAppCompany(appCompany, EnvironmentSetup.Definitions.AppCompany.DefaultValue)
                .ValidateAppPrefix(appPrefix, EnvironmentSetup.Definitions.AppPrefix.DefaultValue);

            // Get soon as possible an instance of the host
            host = builder.PreBuild();

            // Letz continue with the configuration
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddInMemoryCollection();

            if (!isTestHost)
            {
                HostConfigurationManager.LoadHostConfiguration(configBuilder);
                HostConfigurationManager.LoadAppConfiguration(configBuilder);
            }
            else
            {
                HostConfigurationManager.LoadTestConfiguration(configBuilder);
            }
            var config = configBuilder.Build();

            // Load all slim services
            builder.ConfigureServices(services =>
            {
                services
                   .AddLogging(HostLoggingManager.ConfigureSlimLogging)
                   .AddSlimPluginServices()
                   .AddSlimFileServices()
                   .AddSlimVariableServices(config, false);
            });

            // Now get the real instance of the host
            host = builder.Build();

            // Validate the app version
            var envSetup = host.VariableSystem.GetSetup<EnvironmentSetup>();
            builder.ValidateAppVersion(envSetup.AppVersion);

            // Set the environment setup
            envSetup.AppName = host.AppName;
            envSetup.AppCompany = host.AppCompany;
            envSetup.AppPrefix = host.AppPrefix;
        }

        return host;
    }
}
