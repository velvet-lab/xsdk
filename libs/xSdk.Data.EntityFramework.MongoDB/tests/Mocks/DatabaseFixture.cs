using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private MongoDbContainer? mongoDbContainer = null;

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services
                // Add DbContext Factory
                .AddDbContextFactory<TestDbContext>(options =>
                {
                    // Use TestContainers for UnitTests
                    var imageName = GetEnvironmentVariable("MONGODB_IMAGE_NAME");
                    mongoDbContainer = new MongoDbBuilder()
                        .WithImage(imageName)
                        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(27017)))
                        .WithAutoRemove(true)
                        .Build();

                    mongoDbContainer.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var connectionString = mongoDbContainer.GetConnectionString();

                    var client = new MongoClient(connectionString);
                    options.UseMongoDB(client, Globals.DatabaseName);
                })
                .AddDatalayer(builder =>
                {
                    builder
                        .UseEntityFramework<TestDbContext>(
                            Globals.DatalayerName,
                            setup =>
                            {
                                setup.TransactionsEnabled = false;
                            }
                        )
                        .MapRepository<ITestRepository, TestRepository>(Globals.DatalayerName);
                });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                mongoDbContainer?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }
}
