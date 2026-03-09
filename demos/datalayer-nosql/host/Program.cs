using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Data;
using xSdk.Demos.Data;
using xSdk.Demos.Hosting;

const string APP_NAME = "datalayer-nosql";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "dn";

var host = xSdk
    .Hosting.Host.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices(
        (context, services) =>
        {
            services
                // Sample for NoSql Datalayer
                .AddDatalayer(builder =>
                {
                    var datastore = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    builder
                        // Enable FlatFile
                        .UseNoSql(
                            "MyDataStore",
                            config =>
                            {
                                config.Path = datastore;
                                config.FileName = "MyDb.store";
                            }
                        )
                        // Add Repositories to the Layer
                        .MapRepository<ISampleRepository, SampleRepository>();
                })
                .AddHostedService<MyDataHost>();
        }
    )
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
