using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class CrudDatabaseFixture : DatabaseHostFixture
{
    private readonly string _dbName = $"CrudTestDb_{Guid.NewGuid():N}";

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services
                .AddDbContextFactory<TestDbContext>(options =>
                {
                    options
                        .UseInMemoryDatabase(_dbName)
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                })
                .AddDatalayer(builder =>
                {
                    builder
                        .UseEntityFramework<TestDbContext>(Globals.DatalayerName)
                        .MapRepository<ITestRepository, TestRepository>(Globals.DatalayerName);
                });
        });
    }
}
