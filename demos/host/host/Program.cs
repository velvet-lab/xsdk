using xSdk.Demos;
using xSdk.Demos.Hosting;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

const string APP_NAME = "host";
const string APP_COMPANY = "demos";
const string APP_PREFIX = "ho";

var host = xSdk
    .Hosting.Host.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices(
        (context, services) =>
        {
            services
                .AddFileServices()
                .AddVariableServices(setup =>
                {
                    setup
                        .AddEnvironmentVariablesWithoutSetup();
                })
                // Service um Informationen abzurufen
                // Ein eigener Host der benutzt werden soll
                .AddHostedService<MyCustomHost>();
        }
    )
    .RegisterSetup<SetupWithoutPrefix>()
    .RegisterSetup<SetupWithPrefix>()
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
