using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Demos;

const string APP_NAME = "telemetry";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "te";

var host = xSdk.Hosting.Host
    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices((context, services) =>
    {
        services
            .AddSingleton<LocalService>()
            // Service um Informationen abzurufen
            // Ein eigener Host der benutzt werden soll
            .AddHostedService<MyHost>();
    })
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
