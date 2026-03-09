using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using xSdk.Extensions.IO;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private IContainer? _container = null;

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services.AddDatalayer(builder =>
            {
                var currentFolder = Path.Combine(FileSystemHelper.GetExecutingFolder(), "data", Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }

                var imageName = GetEnvironmentVariable("GENERIC_LINUX_IMAGE_NAME");
                _container = new ContainerBuilder()
                    .WithImage(imageName)
                    .WithPortBinding(8080, true)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
                    .WithAutoRemove(true)
                    .WithBindMount(currentFolder, "/data/db")
                    .WithImagePullPolicy(PullPolicy.Missing)
                    .Build();

                _container.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                builder
                    // Enable FlatFile
                    .UseFlatFile(
                        Globals.DatalayerName,
                        config =>
                        {
                            config.FilePath = currentFolder;
                        }
                    )
                    // Add Repositories to the Layer
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
                _container?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }
}
