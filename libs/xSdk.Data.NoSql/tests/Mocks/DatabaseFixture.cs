using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using xSdk.Extensions.IO;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private readonly IContainer? container = null;

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services.AddDatalayer(builder =>
            {
                var currentFolder = Path.Combine(Path.GetTempPath(), "data", Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }

                builder
                    // Enable FlatFile
                    .UseNoSql(
                        Globals.DatalayerName,
                        config =>
                        {
                            config.Path = currentFolder;
                            config.FileName = $"{Globals.DatabaseName}.store";
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
                container?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }
}
