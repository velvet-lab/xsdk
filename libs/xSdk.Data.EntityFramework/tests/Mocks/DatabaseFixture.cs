using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services
                // Add DbContext Factory
                .AddDbContextFactory<TestDbContext>(options =>
                {
                    // Use InMemory Database
                    options
                        .UseInMemoryDatabase(Globals.DatabaseName)
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                })
                .AddDatalayer(builder =>
                {
                    builder
                        .UseEntityFramework<TestDbContext>(Globals.DatalayerName)
                        .MapRepository<ITestRepository, TestRepository>(Globals.DatalayerName)
                        .MapRepository<IConcurrentRepositoryOne, ConcurrentRepositoryOne>(Globals.DatalayerName)
                        .MapRepository<IConcurrentRepositoryTwo, ConcurrentRepositoryTwo>(Globals.DatalayerName);
                });
        });
    }
}
